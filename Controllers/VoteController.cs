using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Mvc;
using Voting_Test.Data;
using Voting_Test.Models;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Voting_Test.Hubs;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Voting_Test.Controllers
{
    public class VoteController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<VoteHub> _hubContext;

        public VoteController(ApplicationDbContext context, IHubContext<VoteHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int PollId, int PollingRoomId)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Redirect("/Identity/Account/Login");
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }

           
            var hasVoted = await _context.Votes
                .AnyAsync(v => v.UserId == userId && v.PollingRoomId == PollingRoomId);

            if (hasVoted)
            {
             
                ModelState.AddModelError(string.Empty, "You have already voted in this polling room.");
                ViewData["PollId"] = new SelectList(_context.Polls, "PollId", "Question", PollId);
                ViewData["PollingRoomId"] = new SelectList(_context.PollingRooms, "PollingRoomId", "Name", PollingRoomId);
                return View();
            }

            var vote = new Vote
            {
                PollId = PollId,
                PollingRoomId = PollingRoomId,
                UserId = userId,
                VoteDate = DateTime.Now
            };

        
            _context.Add(vote);
            await _context.SaveChangesAsync();

            
            await _hubContext.Clients.All.SendAsync("ReceiveVoteUpdate");

           
            return RedirectToAction("PollsByRoom", "Home", new { id = PollingRoomId });
        }
        public async Task<IActionResult> Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Redirect("/Identity/Account/Login");
            }
            var votes = await _context.Votes
                .Include(v => v.Poll)
                .Include(v => v.PollingRoom)
                .Include(v => v.User)
                .ToListAsync();
            return PartialView("_VoteTableRows", votes);
        }



    }
}
