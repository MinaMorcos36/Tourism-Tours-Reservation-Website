using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TourismToursWebsite.Models;

namespace TourismToursWebsite.Controllers
{
    public class ProfileController : Controller
    {
        private readonly TourismToursDbContext _context;

        public ProfileController(TourismToursDbContext context)
        {
            _context = context;
        }

        // GET: Profile/Index (View Profile)
        public async Task<IActionResult> Index()
        {
            var user = new User
            {
                Id = 1,
                FullName = "Ahmed Ali",
                Email = "ahmed@example.com",
                Phone = "0123456789"
            };

            return View(user);
        }

        // GET: Profile/Edit (Edit Profile)
        [HttpGet]
        public async Task<IActionResult> Edit()
        {

            var user = await _context.Users.FirstOrDefaultAsync(u => u.FullName == User.Identity.Name);
            if (user == null)
                return NotFound();

            return View(user);
        }

        // POST: Profile/Edit (Update Profile)
        [HttpPost]
        public async Task<IActionResult> Edit(User user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }

            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == user.Id);
            if (existingUser == null)
                return NotFound();

            // Update user properties (without changing the password here)
            existingUser.FullName = user.FullName;
            existingUser.Email = user.Email;
            existingUser.Phone = user.Phone;

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}