using Microsoft.AspNetCore.Mvc;
using SpaceDash.Models;
using System.Linq;

namespace SpaceDash.Controllers
{
    public class TeamController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TeamController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Create a new team
        [HttpPost]
        public IActionResult Create(string teamName, string gamePin)
        {
            var team = new Team
            {
                Name = teamName,
                GamePin = gamePin,
                Players = new List<Player>() // Empty team to start
            };

            _context.Teams.Add(team);
            _context.SaveChanges();

            return RedirectToAction("Details", new { id = team.Id });
        }

        // View team details
        public IActionResult Details(int id)
        {
            var team = _context.Teams.Find(id);
            if (team == null) return NotFound();

            return View(team);
        }

        // Join a team with a game pin
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
