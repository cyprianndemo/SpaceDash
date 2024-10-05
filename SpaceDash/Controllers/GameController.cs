using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpaceDash.Models;
using System.Collections.Generic;
using System.Linq;

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

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult StartGame(int teamId)
        {
            _logger.LogInformation($"StartGame action called with teamId: {teamId}");
            var team = _context.Teams.Find(teamId);
            if (team == null)
            {
                _logger.LogWarning($"Team not found for ID: {teamId}");
                return NotFound();
            }
            return View(team);
        }

        [HttpPost]
        public IActionResult InitiateGame(int teamId)
        {
            _logger.LogInformation($"InitiateGame action called with teamId: {teamId}");
            var team = _context.Teams.Find(teamId);
            if (team == null)
            {
                _logger.LogWarning($"Team not found for ID: {teamId}");
                return NotFound();
            }

            var firstChallenge = _context.Challenges.OrderBy(c => c.Order).FirstOrDefault();
            if (firstChallenge == null)
            {
                _logger.LogWarning("No challenges found in the database");
                return NotFound("No challenges found.");
            }

            var gameSession = new GameSession
            {
                TeamId = teamId,
                Team = team,
                CurrentChallengeId = firstChallenge.Id,
                CurrentChallenge = firstChallenge,
                StartTime = DateTime.Now
            };

            _context.GameSessions.Add(gameSession);
            _context.SaveChanges();

            _logger.LogInformation($"Game session created with ID: {gameSession.Id}");
            return RedirectToAction("Play", new { sessionId = gameSession.Id });
        }

        public IActionResult Play(int sessionId)
        {
            var session = _context.GameSessions
                .Include(gs => gs.CurrentChallenge)
                .ThenInclude(c => c.Questions)
                .ThenInclude(q => q.Answers)
                .FirstOrDefault(gs => gs.Id == sessionId);

            if (session == null) return NotFound();

            switch (session.CurrentChallenge.Type)
            {
                case "Trivia":
                    return View("Play", session);
                case "Sudoku":
                    return RedirectToAction("Sudoku", new { sessionId });
                case "HighFive":
                    return RedirectToAction("HighFive", new { sessionId });
                default:
                    return NotFound();
            }
        }

        // Submit answer for trivia challenges
        [HttpPost]
        public IActionResult SubmitAnswer(int sessionId, int answerId)
        {
            var session = _context.GameSessions
                .Include(gs => gs.CurrentChallenge)
                .ThenInclude(c => c.Questions)
                .FirstOrDefault(gs => gs.Id == sessionId);

            if (session == null) return NotFound();

            var correctAnswer = session.CurrentChallenge.Questions.FirstOrDefault()?.CorrectAnswerId;
            if (correctAnswer == null) return NotFound("No correct answer found.");

            if (answerId == correctAnswer)
            {
                session.Score += 10;
                session.TimeReward += 5;
            }
            else
            {
                session.TimeReward -= 5;
            }

            session.CurrentChallenge.IsCompleted = true;
            _context.SaveChanges();

            return RedirectToAction("NextChallenge", new { sessionId });
        }

        // Move to the next challenge
        public IActionResult NextChallenge(int sessionId)
        {
            var session = _context.GameSessions
                .Include(gs => gs.CurrentChallenge)
                .FirstOrDefault(gs => gs.Id == sessionId);

            if (session == null) return NotFound();

            var nextChallenge = _context.Challenges
                .Where(c => c.Order > session.CurrentChallenge.Order)
                .OrderBy(c => c.Order)
                .FirstOrDefault();

            if (nextChallenge == null)
            {
                // Game is finished, show scoreboard
                return RedirectToAction("Scoreboard", new { teamId = session.TeamId });
            }

            session.CurrentChallengeId = nextChallenge.Id;
            _context.SaveChanges();

            return RedirectToAction("Play", new { sessionId });
        }

        // Display Sudoku challenge
        public IActionResult Sudoku(int sessionId)
        {
            var gameSession = _context.GameSessions
                .Include(gs => gs.CurrentChallenge)
                .FirstOrDefault(gs => gs.Id == sessionId);

            if (gameSession == null) return NotFound();

            return View(gameSession);
        }

        [HttpPost]
        public IActionResult SubmitSudoku(int sessionId, Dictionary<int, Dictionary<int, string>> cell)
        {
            var gameSession = _context.GameSessions
                .Include(gs => gs.CurrentChallenge)
                .FirstOrDefault(gs => gs.Id == sessionId);

            if (gameSession == null) return NotFound();

            bool isSolved = ValidateSudoku(cell);

            if (isSolved)
            {
                gameSession.CurrentChallenge.IsCompleted = true;
                gameSession.Score += 100;
                _context.SaveChanges();
                return RedirectToAction("NextChallenge", new { sessionId });
            }
            else
            {
                ViewData["Message"] = "Incorrect solution. Please try again!";
                return View("Sudoku", gameSession);
            }
        }

        public IActionResult HighFive(int sessionId)
        {
            var gameSession = _context.GameSessions
                .Include(gs => gs.CurrentChallenge)
                .FirstOrDefault(gs => gs.Id == sessionId);

            if (gameSession == null) return NotFound();

            return View(gameSession);
        }

        [HttpPost]
        public IActionResult SubmitHighFive(int sessionId, int highFiveCount)
        {
            var gameSession = _context.GameSessions
                .Include(gs => gs.CurrentChallenge)
                .FirstOrDefault(gs => gs.Id == sessionId);

            if (gameSession == null) return NotFound();

            if (highFiveCount >= gameSession.CurrentChallenge.RequiredHighFives)
            {
                gameSession.CurrentChallenge.IsCompleted = true;
                gameSession.Score += 50;
                _context.SaveChanges();
                return RedirectToAction("NextChallenge", new { sessionId });
            }
            else
            {
                ViewData["Message"] = "You didn't reach enough high-fives. Try again!";
                return View("HighFive", gameSession);
            }
        }

        private bool ValidateSudoku(Dictionary<int, Dictionary<int, string>> grid)
        {
            // Implement your Sudoku validation logic here
            return true; // For now, assume it's always correct
        }

        // Display the scoreboard at the end of the game
        public IActionResult Scoreboard(int teamId)
        {
            var team = _context.Teams
                .Include(t => t.Players)
                .FirstOrDefault(t => t.Id == teamId);

            if (team == null) return NotFound();

            return View(team);
        }
    }
}