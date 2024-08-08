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
        var rooms = await _context.PollingRooms.ToListAsync();
        return View(rooms);
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
            .Include(r => r.Polls) // Include related polls
            .FirstOrDefaultAsync(r => r.PollingRoomId == id);
        if (room == null)
        {
            return NotFound();
        }

        // Remove associated polls first
        _context.Polls.RemoveRange(room.Polls);

        // Remove polling room
        _context.PollingRooms.Remove(room);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }


    private bool PollingRoomExists(int id)
    {
        return _context.PollingRooms.Any(e => e.PollingRoomId == id);
    }

    // Add methods for handling polls within a specific polling room
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
