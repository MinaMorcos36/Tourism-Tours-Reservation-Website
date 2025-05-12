using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TourismToursWebsite.Models;

namespace TourismToursWebsite.Controllers
{
    [Authorize] // Only logged-in users can access reservations
    public class ReservationsController : Controller
    {
        private readonly TourismToursDbContext _context;

        public ReservationsController(TourismToursDbContext context)
        {
            _context = context;
        }

        // GET: Reservations (Reservation Confirmation Page)
        public async Task<IActionResult> Index(int? id)
        {
            if (id == null)
            {
                return NotFound("Tour ID is missing.");
            }

            var tour = await _context.Tours.FirstOrDefaultAsync(t => t.Id == id);
            if (tour == null)
            {
                return NotFound("Tour not found.");
            }

            // Pass the tour information to the Reservation Page
            ViewBag.Tour = tour;
            return View();
        }

        // POST: Reservations/Create (Booking Confirmation)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int TourId, string FullName, string Email, string PhoneNumber)
        {
            if (string.IsNullOrEmpty(FullName))
            {
                return BadRequest("Full Name is required.");
            }

            var tour = await _context.Tours.FirstOrDefaultAsync(t => t.Id == TourId);
            if (tour == null)
            {
                return NotFound("Tour not found.");
            }

            // Save the reservation
            var reservation = new Reservation
            {
                TourId = TourId,
                UserName = FullName,
                UserEmail = Email,
                UserPhone = PhoneNumber,
                ReservationDate = DateOnly.FromDateTime(DateTime.Now)
            };

            _context.Add(reservation);
            await _context.SaveChangesAsync();

            // Redirect to Cart after reserving
            return RedirectToAction("Index", "Cart");
        }

        // GET: Reservations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations
                .Include(r => r.Tour)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // POST: Reservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation != null)
            {
                _context.Reservations.Remove(reservation);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index", "Cart");
        }

        private bool ReservationExists(int id)
        {
            return _context.Reservations.Any(e => e.Id == id);
        }
    }
}
