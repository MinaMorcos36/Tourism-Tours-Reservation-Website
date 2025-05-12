using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TourismToursWebsite.Models;

namespace TourismToursWebsite.Controllers
{
    [Authorize] // Only logged-in users can access their cart
    public class CartController : Controller
    {
        private readonly TourismToursDbContext _context;

        public CartController(TourismToursDbContext context)
        {
            _context = context;
        }

        // GET: Cart (List all reserved tours for the user)
        public async Task<IActionResult> Index()
        {
            var cartItems = new List<Tour>
        {
            new Tour { Id = 1, Name = "Program3", Description = "Visit the ship wrecks in Sharm El Sheikh", Price = 150.00M },
            new Tour { Id = 2, Name = "Program7", Description = "Enjoy a luxury yacht in Hurghada", Price = 250.00M },
            new Tour { Id = 3, Name = "Program16", Description = "Adventure in the Sahara Desert in Dahab", Price = 300.00M }
        };

            return View(cartItems);
        }
    }
}
