using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpaceDash.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SpaceDash.Controllers
{
    public class GameController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<GameController> _logger;

        public GameController(ApplicationDbContext context, ILogger<GameController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Lobby(int teamId)
        {
            var gameSession = await _context.GameSessions
                .Include(gs => gs.Team)
                .Include(gs => gs.CurrentChallenge)
                .FirstOrDefaultAsync(gs => gs.TeamId == teamId && !gs.IsCompleted);

            if (gameSession == null)
            {
                return NotFound("No active game session found.");
            }

            return View(gameSession);
        }

        private string GenerateSudokuPuzzle()
        {
            Random random = new Random();
            char[] puzzle = new char[36];

            for (int i = 0; i < puzzle.Length; i++)
            {
                puzzle[i] = (random.Next(0, 6) + '1').ToString()[0];
            }

            return new string(puzzle);
        }

        private readonly string[] TriviaQuestions = new string[]
        {
            "What is the capital of Kenya?",
            "What is the largest planet in our solar system?",
            "Who wrote 'Romeo and Juliet'?",
            "What is the chemical symbol for gold?",
            "Who was the first president of the United States?",
            "Which element is needed for combustion?",
            "What year did World War II end?",
            "What is the hardest natural substance on Earth?",
            "What is the tallest mountain in the world?",
            "Who painted the Mona Lisa?"
        };

        private readonly string[] TriviaAnswers = new string[]
        {
            "Nairobi", "Jupiter", "Shakespeare", "Au", "George Washington",
            "Oxygen", "1945", "Diamond", "Mount Everest", "Leonardo da Vinci"
        };

        public async Task<IActionResult> Play(int sessionId)
        {
            var gameSession = await _context.GameSessions
                .Include(gs => gs.CurrentChallenge)
                .FirstOrDefaultAsync(gs => gs.Id == sessionId);

            if (gameSession == null)
            {
                return NotFound("Game session not found.");
            }

            if (gameSession.CurrentChallenge == null)
            {
                return RedirectToAction(nameof(NextChallenge), new { sessionId });
            }

            if (gameSession.CurrentChallenge.Type == "Sudoku")
            {
                if (string.IsNullOrEmpty(gameSession.CurrentChallenge.SudokuPuzzle))
                {
                    gameSession.CurrentChallenge.SudokuPuzzle = GenerateSudokuPuzzle();
                    gameSession.CurrentChallenge.Solution = gameSession.CurrentChallenge.SudokuPuzzle;
                    gameSession.CurrentChallenge.TimeLimit = 120;
                    await _context.SaveChangesAsync();
                }

                ViewBag.StartTime = DateTime.UtcNow;
            }

            return View(gameSession.CurrentChallenge.Type, gameSession);
        }

        [HttpPost]
        public async Task<IActionResult> SubmitSudoku(int sessionId, string solution, DateTime startTime)
        {
            var gameSession = await _context.GameSessions
                .Include(gs => gs.CurrentChallenge)
                .FirstOrDefaultAsync(gs => gs.Id == sessionId);

            if (gameSession == null || gameSession.CurrentChallenge.Type != "Sudoku")
            {
                return NotFound();
            }

            var endTime = DateTime.UtcNow;
            var timeTaken = (endTime - startTime).TotalSeconds;

            if (solution == gameSession.CurrentChallenge.Solution)
            {
                if (timeTaken <= gameSession.CurrentChallenge.TimeLimit)
                {
                    gameSession.Score += 100;
                    gameSession.TimeReward += 10;
                    gameSession.CurrentChallenge.IsCompleted = true;
                    await _context.SaveChangesAsync();

                    // Generate a random room number
                    Random random = new Random();
                    int roomNumber = random.Next(1, 6); // Assuming 5 rooms

                    // Store the room number and start time in TempData
                    TempData["RoomNumber"] = roomNumber;
                    TempData["RoomChallengeStartTime"] = DateTime.UtcNow.ToString("O");

                    return RedirectToAction(nameof(RoomChallenge), new { sessionId, roomNumber });
                }
                else
                {
                    return RedirectToAction(nameof(NextChallenge), new { sessionId, message = "Time's up! Moving to the next challenge." });
                }
            }
            else
            {
                gameSession.TimeReward -= 5;
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(NextChallenge), new { sessionId, message = "Incorrect solution. Moving to the next challenge." });
            }
        }

        public IActionResult RoomChallenge(int sessionId, int roomNumber)
        {
            ViewBag.RoomNumber = roomNumber;
            ViewBag.SessionId = sessionId;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> VerifyRoomChallenge(int sessionId, int roomNumber)
        {
            if (TempData["RoomNumber"] == null || TempData["RoomChallengeStartTime"] == null)
            {
                return RedirectToAction(nameof(NextChallenge), new { sessionId, message = "Room challenge data not found. Moving to the next challenge." });
            }

            int correctRoomNumber = (int)TempData["RoomNumber"];
            DateTime startTime;

            // Parse the string back to DateTime
            if (!DateTime.TryParse(TempData["RoomChallengeStartTime"].ToString(), out startTime))
            {
                return RedirectToAction(nameof(NextChallenge), new { sessionId, message = "Invalid start time. Moving to the next challenge." });
            }

            DateTime endTime = DateTime.UtcNow;

            var timeTaken = (endTime - startTime).TotalSeconds;

            var gameSession = await _context.GameSessions.FindAsync(sessionId);

            if (gameSession == null)
            {
                return NotFound();
            }

            if (roomNumber == correctRoomNumber && timeTaken <= 20)
            {
                gameSession.Score += 150;
                gameSession.TimeReward += 15;
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(NextChallenge), new { sessionId, message = "Room challenge completed successfully! Moving to the next challenge." });
            }
            else
            {
                gameSession.TimeReward -= 10;
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(NextChallenge), new { sessionId, message = "Room challenge failed. Moving to the next challenge." });
            }
        }

        public async Task<IActionResult> Trivia(int sessionId, int questionIndex = 0)
        {
            var gameSession = await _context.GameSessions
                .Include(gs => gs.CurrentChallenge)
                .FirstOrDefaultAsync(gs => gs.Id == sessionId);

            if (gameSession == null || gameSession.CurrentChallenge.Type != "Trivia")
            {
                return NotFound("Trivia challenge not found.");
            }

            if (questionIndex >= TriviaQuestions.Length)
            {
                return RedirectToAction(nameof(NextChallenge), new { sessionId });
            }

            ViewBag.Question = TriviaQuestions[questionIndex];
            ViewBag.QuestionIndex = questionIndex;
            ViewBag.StartTime = DateTime.UtcNow;

            return View(gameSession);
        }

         [HttpPost]
        public async Task<IActionResult> SubmitTrivia(int sessionId, string answer, int questionIndex, DateTime startTime)
        {
            var gameSession = await _context.GameSessions
                .Include(gs => gs.CurrentChallenge)
                .FirstOrDefaultAsync(gs => gs.Id == sessionId);

            if (gameSession == null || gameSession.CurrentChallenge.Type != "Trivia")
            {
                return NotFound();
            }

            var endTime = DateTime.UtcNow;
            var timeTaken = (endTime - startTime).TotalSeconds;

            if (timeTaken > 5)
            {
                return RedirectToAction(nameof(Trivia), new { sessionId, questionIndex = questionIndex + 1 });
            }

            if (answer?.Trim().Equals(TriviaAnswers[questionIndex], StringComparison.OrdinalIgnoreCase) == true)
            {
                gameSession.Score += 50;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Trivia), new { sessionId, questionIndex = questionIndex + 1 });
        }


        [HttpPost]
        public async Task<IActionResult> SubmitHighFive(int sessionId)
        {
            var gameSession = await _context.GameSessions
                .Include(gs => gs.CurrentChallenge)
                .FirstOrDefaultAsync(gs => gs.Id == sessionId);

            if (gameSession == null || gameSession.CurrentChallenge.Type != "HighFive")
            {
                return NotFound();
            }

            gameSession.CurrentChallenge.RequiredHighFives--;

            if (gameSession.CurrentChallenge.RequiredHighFives <= 0)
            {
                gameSession.Score += 50;
                gameSession.TimeReward += 15;
                gameSession.CurrentChallenge.IsCompleted = true;
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(NextChallenge), new { sessionId });
            }

            await _context.SaveChangesAsync();
            return View("HighFive", gameSession);
        }

        public async Task<IActionResult> NextChallenge(int sessionId, string message = null)
        {
            var gameSession = await _context.GameSessions
                .Include(gs => gs.Challenges)
                .FirstOrDefaultAsync(gs => gs.Id == sessionId);

            if (gameSession == null)
            {
                return NotFound();
            }

            var nextChallenge = gameSession.Challenges
                .OrderBy(c => c.Order)
                .FirstOrDefault(c => !c.IsCompleted);

            if (nextChallenge == null)
            {
                gameSession.IsCompleted = true;
                gameSession.EndTime = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(GameOver), new { sessionId });
            }

            gameSession.CurrentChallengeId = nextChallenge.Id;
            await _context.SaveChangesAsync();

            ViewBag.Message = message;
            return RedirectToAction(nameof(Play), new { sessionId });
        }

        public async Task<IActionResult> GameOver(int sessionId)
        {
            var gameSession = await _context.GameSessions
                .Include(gs => gs.Team)
                .FirstOrDefaultAsync(gs => gs.Id == sessionId);

            if (gameSession == null)
            {
                return NotFound();
            }

            return View(gameSession);
        }
    }
}
