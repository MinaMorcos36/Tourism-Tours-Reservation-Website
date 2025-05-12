using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using TourismToursWebsite.Models;
using Microsoft.EntityFrameworkCore;

namespace TourismToursWebsite.Controllers
{
    public class UserController : Controller
    {
        private readonly TourismToursDbContext _context;
        private readonly PasswordHasher<User> _passwordHasher;

        public UserController(TourismToursDbContext context)
        {
            _context = context;
            _passwordHasher = new PasswordHasher<User>();
        }

        // GET: Login Page
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: Login User
        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                ViewBag.ErrorMessage = "Invalid Email or Password.";
                return View();
            }

            // Verify password
            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
            if (result == PasswordVerificationResult.Failed)
            {
                ViewBag.ErrorMessage = "Invalid Email or Password.";
                return View();
            }

            // Set up user claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Email, user.Email ?? ""),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties { IsPersistent = true };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity), authProperties);

            // Redirect based on role
            if (user.Role == "Admin")
                return RedirectToAction("Index", "Home");
            else
                return RedirectToAction("Index", "Home");
        }

        // GET: Register Page
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // POST: Register User
        [HttpPost]
        public async Task<IActionResult> Register([Bind("FullName,Email,Phone")] User user, string password)
        {
            if (string.IsNullOrWhiteSpace(user.FullName) ||
                string.IsNullOrWhiteSpace(user.Email) ||
                string.IsNullOrWhiteSpace(user.Phone) ||
                string.IsNullOrWhiteSpace(password))
            {
                ViewBag.ErrorMessage = "Please fill all required fields.";
                return View();
            }

            // Check if email already exists
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
            if (existingUser != null)
            {
                ViewBag.ErrorMessage = "Email is already registered.";
                return View();
            }

            // Hash the password
            user.PasswordHash = _passwordHasher.HashPassword(user, password);
            user.Role = "Customer"; // Default role is Customer

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return RedirectToAction("Login");
        }


        // Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}