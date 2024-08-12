using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;
using Voting_Test.Data;
using Voting_Test.Models;

namespace Voting_Test.Controllers
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

        public async Task<IActionResult> Index()
        {
            if(!User.Identity.IsAuthenticated)
            {
                return Redirect("/Identity/Account/Login");
            }
            // Fetch polling rooms
            var pollingRooms = await _context.PollingRooms.ToListAsync();

            return View(pollingRooms);
        }


        public async Task<IActionResult> PollsByRoom(int id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Redirect("/Identity/Account/Login");
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var viewModel = new PollsByRoomViewModel
            {
                PollingRoomId = id,
                Polls = await _context.Polls
                    .Where(p => p.PollingRoomId == id && (p.EndDate == null || p.EndDate > DateTime.Now))
                    .ToListAsync(),
                UserHasVoted = await _context.Votes
                    .AnyAsync(v => v.UserId == userId && v.PollingRoomId == id)
            };

            return View(viewModel);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
