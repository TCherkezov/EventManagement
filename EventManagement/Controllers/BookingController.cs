using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EventManagement.Data;
using EventManagement.Models;
using System.Linq;
using System.Threading.Tasks;

[Authorize]
public class BookingController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public BookingController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<IActionResult> Create(int eventId)
    {
        var @event = await _context.Events.FindAsync(eventId);
        if (@event == null)
        {
            return View(new Booking { Event = null });
        }

        var booking = new Booking
        {
            EventID = eventId,
            Event = @event,
            UserID = _userManager.GetUserId(User),
            BookingDate = DateTime.Now
        };

        return View(booking);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("EventID,UserID,Quantity,BookingDate")] Booking booking)
    {
        //if (ModelState.IsValid)
        
            _context.Add(booking);
            await _context.SaveChangesAsync();
            return RedirectToAction("UserBookings");
        
        return View(booking);
    }

    public async Task<IActionResult> UserBookings()
    {
        var userId = _userManager.GetUserId(User);
        var bookings = await _context.Bookings
            .Include(b => b.Event)
            .Where(b => b.UserID == userId)
            .ToListAsync();
        return View(bookings);
    }
}

