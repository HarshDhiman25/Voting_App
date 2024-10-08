﻿using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Mvc;
using Voting_Test.Data;
using Voting_Test.Models;
using Voting_Test.Models.ViewModels; // Corrected namespace
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
            await _hubContext.Clients.All.SendAsync("ReceiveVoteUpdate"); // Ensure this line is present

            return RedirectToAction("PollsByRoom", "Home", new { id = PollingRoomId });
        }




        public async Task<IActionResult> Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Redirect("/Identity/Account/Login");
            }

            var pollsToCheck = await _context.Polls
                .Where(p => p.EndDate <= DateTime.Now && !_context.PollWinnerss.Any(w => w.PollId == p.PollId))
                .ToListAsync();

            foreach (var poll in pollsToCheck)
            {
                var voteCount = await _context.Votes
                    .Where(v => v.PollId == poll.PollId)
                    .CountAsync();

                if (voteCount > 0)
                {
                    var winner = new PollWinners
                    {
                        PollId = poll.PollId,
                        PollQuestion = poll.Question,
                        PollingRoomName = (await _context.PollingRooms.FindAsync(poll.PollingRoomId))?.Name,
                        TotalVotes = voteCount,
                        WinnerMessage = $"This {poll.Question} is the winner!",
                        WinningDate = DateTime.Now
                    };

                    _context.PollWinnerss.Add(winner);
                    await _context.SaveChangesAsync();
                }
            }

            var votes = await _context.Votes
                .Include(v => v.Poll)
                .Include(v => v.PollingRoom)
                .Include(v => v.User)
                .ToListAsync();

            var voteStats = votes
                .GroupBy(v => v.PollId)
                .Select(group => new VoteStatisticsViewModel
                {
                    PollId = group.Key,
                    PollQuestion = group.First().Poll.Question,
                    PollingRoomName = group.First().PollingRoom.Name,
                    TotalVotes = group.Count(),
                })
                .OrderByDescending(v => v.TotalVotes)
                .ToList();

            ViewBag.VoteStats = voteStats;

            // Retrieve winners
            var pollWinners = await _context.PollWinnerss.ToListAsync();
            ViewBag.PollWinners = pollWinners;

            return View(votes);
        }


    }
}
