using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Voting_Test.Data;
using Voting_Test.Models;

public class PollingRoomController : Controller
{
    private readonly ApplicationDbContext _context;

    public PollingRoomController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        if (!User.Identity.IsAuthenticated)
        {
            return Redirect("/Identity/Account/Login");
        }

        var currentDate = DateTime.Now;

        var allPollingRooms = await _context.PollingRooms.ToListAsync();

        foreach (var room in allPollingRooms.Where(r => r.EndDate < currentDate && r.IsActive))
        {
            room.IsActive = false;
            _context.Update(room);
        }

        var activePollingRooms = allPollingRooms.Where(r => r.IsActive).ToList();
        await _context.SaveChangesAsync();

        return View(activePollingRooms);
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
    public async Task<IActionResult> Create([Bind("Name,IsActive")] PollingRoom room)
    {
        if (ModelState.IsValid)
        {
            room.CreatedDate = DateTime.Now;
            _context.Add(room);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(room);
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

        var room = await _context.PollingRooms.FindAsync(id);
        if (room == null)
        {
            return NotFound();
        }
        return View(room);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("PollingRoomId,Name,CreatedDate,IsActive")] PollingRoom room)
    {
        if (id != room.PollingRoomId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(room);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PollingRoomExists(room.PollingRoomId))
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
        return View(room);
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

        var room = await _context.PollingRooms
            .FirstOrDefaultAsync(m => m.PollingRoomId == id);
        if (room == null)
        {
            return NotFound();
        }

        return View(room);
    }

    [HttpPost, ActionName("DeleteConfirmed")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var room = await _context.PollingRooms
            .Include(r => r.Polls)
            .FirstOrDefaultAsync(r => r.PollingRoomId == id);
        if (room == null)
        {
            return NotFound();
        }

        _context.Polls.RemoveRange(room.Polls);
        _context.PollingRooms.Remove(room);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    private bool PollingRoomExists(int id)
    {
        return _context.PollingRooms.Any(e => e.PollingRoomId == id);
    }

    private bool PollingExists(int id)
    {
        return _context.Polls.Any(e => e.PollId == id);
    }

    public async Task<IActionResult> Polls(int id)
    {
        if (!User.Identity.IsAuthenticated)
        {
            return Redirect("/Identity/Account/Login");
        }
        var room = await _context.PollingRooms.FindAsync(id);
        if (room == null)
        {
            return NotFound();
        }

        var polls = await _context.Polls
            .Where(p => p.PollingRoomId == id)
            .ToListAsync();

        ViewData["PollingRoomId"] = id;
        ViewData["PollingRoomName"] = room.Name;

        return View(polls);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditPoll(int id)
    {
        if (!User.Identity.IsAuthenticated)
        {
            return Redirect("/Identity/Account/Login");
        }
        var poll = await _context.Polls.FindAsync(id);
        if (poll == null)
        {
            return NotFound();
        }

        ViewData["PollingRoomId"] = poll.PollingRoomId;

        return View(poll);
    }

    public IActionResult CreatePoll(int pollingRoomId)
    {
        if (!User.Identity.IsAuthenticated)
        {
            return Redirect("/Identity/Account/Login");
        }
        ViewData["PollingRoomId"] = pollingRoomId;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreatePoll([Bind("Question,EndDate,IsActive")] Poll poll, int pollingRoomId)
    {
        if (ModelState.IsValid)
        {
            poll.CreatedDate = DateTime.Now;
            poll.PollingRoomId = pollingRoomId;
            _context.Add(poll);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Polls), new { id = pollingRoomId });
        }

        ViewData["PollingRoomId"] = pollingRoomId;
        return View(poll);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeletePoll(int id)
    {
        if (!User.Identity.IsAuthenticated)
        {
            return Redirect("/Identity/Account/Login");
        }
        var poll = await _context.Polls.FindAsync(id);
        if (poll == null)
        {
            return NotFound();
        }

        _context.Polls.Remove(poll);
        await _context.SaveChangesAsync();

        return Json(new { success = true, message = "Poll deleted successfully." });
    }
}
