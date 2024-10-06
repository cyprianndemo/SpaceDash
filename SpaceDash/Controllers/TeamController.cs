using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpaceDash.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpaceDash.Controllers
{
    public class TeamController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TeamController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(string teamName, string gamePin)
        {
            if (string.IsNullOrEmpty(gamePin))
            {
                gamePin = GenerateGamePin();
            }

            var team = new Team
            {
                Name = teamName,
                GamePin = gamePin
            };

            _context.Teams.Add(team);
            await _context.SaveChangesAsync();

            var gameSession = await CreateGameSessionForTeam(team.Id);

            return RedirectToAction("Lobby", "Game", new { teamId = team.Id });
        }

        private async Task<GameSession> CreateGameSessionForTeam(int teamId)
        {
            var device = await _context.Devices.FirstOrDefaultAsync() ?? await CreateDefaultDevice();

            var gameSession = new GameSession
            {
                TeamId = teamId,
                StartTime = DateTime.UtcNow,
                EndTime = null,
                IsCompleted = false,
                Score = 0,
                TimeReward = 0
            };

            _context.GameSessions.Add(gameSession);
            await _context.SaveChangesAsync();

            // Create challenges after saving the GameSession
            await CreateChallengesForGameSession(gameSession.Id, device.Id);

            // Update the CurrentChallengeId with the first challenge
            var firstChallenge = await _context.Challenges
                .Where(c => c.GameSessionId == gameSession.Id)
                .OrderBy(c => c.Order)
                .FirstOrDefaultAsync();

            if (firstChallenge != null)
            {
                gameSession.CurrentChallengeId = firstChallenge.Id;
                await _context.SaveChangesAsync();
            }

            return gameSession;
        }

        private async Task<Device> CreateDefaultDevice()
        {
            var device = new Device
            {
                Name = "Default Device",
                Location = "Unknown"
            };
            _context.Devices.Add(device);
            await _context.SaveChangesAsync();
            return device;
        }

        private async Task CreateChallengesForGameSession(int gameSessionId, int deviceId)
        {
            var challenges = new List<Challenge>
            {
                new Challenge
                {
                    Name = "Sudoku Challenge",
                    Description = "Solve this Sudoku puzzle.",
                    Type = "Sudoku",
                    Order = 1,
                    GameSessionId = gameSessionId,
                    DeviceId = deviceId,
                    IsCompleted = false
                },
                new Challenge
                {
                    Name = "High Five Challenge",
                    Description = "Give a high five to your teammate!",
                    Type = "HighFive",
                    Order = 2,
                    GameSessionId = gameSessionId,
                    DeviceId = deviceId,
                    RequiredHighFives = 1,
                    IsCompleted = false
                }
            };

            _context.Challenges.AddRange(challenges);
            await _context.SaveChangesAsync();
        }

        private string GenerateGamePin()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, 6)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public IActionResult Details(int id)
        {
            var team = _context.Teams.Find(id);
            if (team == null) return NotFound();

            return View(team);
        }

        [HttpPost]
        public IActionResult Join(string gamePin, string playerName)
        {
            var team = _context.Teams.FirstOrDefault(t => t.GamePin == gamePin);
            if (team == null) return NotFound("Invalid game pin");

            var player = new Player
            {
                Name = playerName,
                TeamId = team.Id,
                Team = team
            };

            _context.Players.Add(player);
            _context.SaveChanges();

            return RedirectToAction("Details", new { id = team.Id });
        }
    }
}