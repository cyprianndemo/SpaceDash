using Microsoft.AspNetCore.Mvc;
using SpaceDash.Models;
using System;
using System.Diagnostics;

namespace SpaceDash.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateTeam(Team team)
        {
            if (ModelState.IsValid)
            {
                team.GamePin = GenerateGamePin();
                _context.Teams.Add(team);
                _context.SaveChanges();

                return RedirectToAction("Lobby", "Game", new { teamId = team.Id });
            }

            return View("Index", team);
        }

        private string GenerateGamePin()
        {
            return new Random().Next(100000, 999999).ToString();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}