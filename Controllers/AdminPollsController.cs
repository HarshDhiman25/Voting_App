using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Voting_Test.Data;
using Voting_Test.Models;

[Authorize]
public class AdminPollsController : Controller
{
    private readonly ApplicationDbContext _context;

    public AdminPollsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: AdminPolls
    public async Task<IActionResult> Index()
    {
        if (!User.Identity.IsAuthenticated)
        {
            return Redirect("/Identity/Account/Login");
        }

        var currentDate = DateTime.Now;

        // Get all polls
        var allPolls = await _context.Polls.ToListAsync();

        // Disable expired polls
        foreach (var poll in allPolls.Where(p => p.EndDate < currentDate && p.IsActive))
        {
            poll.IsActive = false;
            _context.Update(poll);
        }
        await _context.SaveChangesAsync();

        // Fetch active polls
        var polls = allPolls.Where(p => p.EndDate >= currentDate && p.IsActive).ToList();

        return View(polls);
    }

    public IActionResult Create()
    {
        if (!User.Identity.IsAuthenticated)
        {
            return Redirect("/Identity/Account/Login");
        }
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Question,EndDate,IsActive")] Poll? poll)
    {
        if (ModelState.IsValid)
        {
            poll.CreatedDate = DateTime.Now;
            _context.Add(poll);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(poll);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (!User.Identity.IsAuthenticated)
        {
            return Redirect("/Identity/Account/Login");
        }
        if (id == null)
        {
            return NotFound();
        }

        var poll = await _context.Polls.FindAsync(id);
        if (poll == null)
        {
            return NotFound();
        }
        return View(poll);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditPoll(int id, [Bind("PollId,Question,EndDate,IsActive")] Poll poll)
    {
        if (id != poll.PollId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(poll);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PollExists(poll.PollId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }

        return View(poll);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (!User.Identity.IsAuthenticated)
        {
            return Redirect("/Identity/Account/Login");
        }
        if (id == null)
        {
            return NotFound();
        }

        var poll = await _context.Polls
            .FirstOrDefaultAsync(m => m.PollId == id);
        if (poll == null)
        {
            return NotFound();
        }

        return View(poll);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var poll = await _context.Polls.FindAsync(id);
        if (poll == null)
        {
            return NotFound();
        }

        _context.Polls.Remove(poll);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool PollExists(int id)
    {
        return _context.Polls.Any(e => e.PollId == id);
    }
}
