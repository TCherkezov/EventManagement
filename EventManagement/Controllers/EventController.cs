using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EventManagement.Data;
using EventManagement.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

[Authorize]
public class EventController : Controller
{
    private readonly ApplicationDbContext _context;

    public EventController(ApplicationDbContext context)
    {
        _context = context;
    }

    
    public async Task<IActionResult> Index()
    {
        var events = await _context.Events.Include(e => e.Category).ToListAsync();
        return View(events);
    }

    
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var @event = await _context.Events
            .Include(e => e.Category)
            .FirstOrDefaultAsync(m => m.EventID == id);

        if (@event == null)
        {
            return NotFound();
        }

        return View(@event);
    }

    
    [Authorize(Roles = "Admin")]
    public IActionResult Create()
    {
        ViewBag.Categories = new SelectList(_context.Categories, "CategoryID", "CategoryName");
        return View();
    }

   
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([Bind("Name,Date,Time,Location,CategoryID,TicketPrice")] Event @event)
    {
        
            _context.Add(@event);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        

        ViewBag.Categories = new SelectList(_context.Categories, "CategoryID", "CategoryName", @event.CategoryID);
        return View(@event);
    }

    
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var @event = await _context.Events.FindAsync(id);
        if (@event == null)
        {
            return NotFound();
        }
        ViewData["CategoryID"] = new SelectList(_context.Categories, "CategoryID", "CategoryName", @event.CategoryID);
        return View(@event);
    }

    
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int id, [Bind("EventID,Name,Date,Time,Location,CategoryID,TicketPrice")] Event @event)
    {

        
                _context.Update(@event);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
        
        ViewData["CategoryID"] = new SelectList(_context.Categories, "CategoryID", "CategoryName", @event.CategoryID);
        return View(@event);
    }

    
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var @event = await _context.Events
            .Include(e => e.Category)
            .FirstOrDefaultAsync(m => m.EventID == id);
        if (@event == null)
        {
            return NotFound();
        }

        return View(@event);
    }

    
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var @event = await _context.Events.FindAsync(id);
        _context.Events.Remove(@event);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool EventExists(int id)
    {
        return _context.Events.Any(e => e.EventID == id);
    }
}
