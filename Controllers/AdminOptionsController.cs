using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Voting_Test.Data;
using Microsoft.CodeAnalysis.Options;
using Voting_Test.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

public class AdminOptionsController : Controller
{
    private readonly ApplicationDbContext _context;

    public AdminOptionsController(ApplicationDbContext context)
    {
        _context = context;
    }


    public IActionResult Create()
    {
        var polls = _context.Polls.ToList();
        if (polls == null || !polls.Any())
        {
            ModelState.AddModelError(string.Empty, "No polls available. Please create a poll first.");
            ViewBag.PollId = new SelectList(new List<Poll>());
        }
        else
        {
            ViewBag.PollId = new SelectList(polls, "Id", "Title");
        }

        return View();
    }

    // POST: AdminOptions/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Text,PollId")] Option option)
    {
        if (ModelState.IsValid)
        {
            _context.Add(option);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        var polls = _context.Polls.ToList();
        if (polls == null || !polls.Any())
        {
            ModelState.AddModelError(string.Empty, "No polls available. Please create a poll first.");
            ViewBag.PollId = new SelectList(new List<Poll>());
        }
        else
        {
            ViewBag.PollId = new SelectList(polls, "Id", "Title", option.PollId);
        }

        return View(option);
    }
    // GET: AdminOptions/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var option = await _context.Options.FindAsync(id);
        if (option == null)
        {
            return NotFound();
        }
        return View(option);
    }

    // POST: AdminOptions/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("OptionId,Text,PollId")] Option option)
    {
        if (id != option.OptionId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(option);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OptionExists(option.OptionId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction("Details", "AdminPolls", new { id = option.PollId });
        }
        return View(option);
    }

    // GET: AdminOptions/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var option = await _context.Options
            .Include(o => o.Poll)
            .FirstOrDefaultAsync(m => m.OptionId == id);
        if (option == null)
        {
            return NotFound();
        }

        return View(option);
    }

    // POST: AdminOptions/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var option = await _context.Options.FindAsync(id);
        _context.Options.Remove(option);
        await _context.SaveChangesAsync();
        return RedirectToAction("Details", "AdminPolls", new { id = option.PollId });
    }

    private bool OptionExists(int id)
    {
        return _context.Options.Any(e => e.OptionId == id);
    }
}
