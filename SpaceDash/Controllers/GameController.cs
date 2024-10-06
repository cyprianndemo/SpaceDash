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
                    return RedirectToAction(nameof(NextChallenge), new { sessionId, message = "Correct! Proceed to the next device." });
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

        public async Task<IActionResult> NextChallenge(int sessionId)
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