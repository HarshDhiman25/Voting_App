using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Voting_Test.Data;
using Voting_Test.Models;

[Authorize] // Apply authorization to ensure only authorized users can access these actions
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
        var polls = await _context.Polls.ToListAsync();
        return View(polls);
    }

    // GET: AdminPolls/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: AdminPolls/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Question,CreatedDate,EndDate,IsActive")] Poll poll)
    {
        if (ModelState.IsValid)
        {
            poll.CreatedDate = DateTime.Now; // Set CreatedDate to now when the poll is created
            _context.Add(poll);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index)); // Redirect to the Index action after successful creation
        }
        return View(poll);
    }

    // GET: AdminPolls/Edit/5
    // GET: AdminPolls/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
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

    // POST: AdminPolls/Edit/5
   // POST: AdminPolls/Edit/5
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Edit(int id, [Bind("PollId,Question,CreatedDate,EndDate,IsActive")] Poll poll)
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



    // GET: AdminPolls/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
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
    // POST: AdminPolls/Delete/5
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

        return Json(new { success = true, message = "Poll deleted successfully." });
    }


    private bool PollExists(int id)
    {
        return _context.Polls.Any(e => e.PollId == id);
    }
}
