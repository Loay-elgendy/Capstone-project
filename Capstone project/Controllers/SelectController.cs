using Capstone_project.Models;
using Capstone_project.data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Capstone_project.Controllers
{
    public class SelectController : Controller
    {
        private readonly context _context;

        public SelectController(context context)
        {
            _context = context;
        }

        // GET: /Select/Select/{id}?userId={userId}
        [HttpGet]
        public async Task<IActionResult> Select(int id, int userId)
        {
            // id = clinic Id, userId = doctor's UserId
            var clinic = await _context.AddClinics.FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);
            if (clinic == null)
            {
                return NotFound();
            }

            ViewBag.ClinicId = id;
            ViewBag.DoctorUserId = userId; // pass to view if needed
            return View(clinic);
        }

        // POST: /Select/Select/{id}?userId={userId}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Select(int id, int userId, string selectedTime, string selectedDay)
        {
            var clinic = await _context.AddClinics.FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);
            if (clinic == null)
            {
                return NotFound();
            }

            if (string.IsNullOrEmpty(selectedTime) || string.IsNullOrEmpty(selectedDay))
            {
                ModelState.AddModelError(string.Empty, "Please select both a time and a day before proceeding.");
                return View(clinic);
            }

            // Get logged-in user's ID to save as PatientId in Select
            var currentUserEmail = User.Identity.Name;

            // Create new Select reservation and save doctorId from URL
            var reservation = new Select
            {
                DoctorId = userId, // doctor's UserId
                DoctorName = clinic.DoctorName,
                Location = clinic.Location,
                Time = selectedTime,
                Day = selectedDay,
            };

            _context.Selects.Add(reservation);
            await _context.SaveChangesAsync();

            // Redirect after successful booking
            return RedirectToAction("Status", "Status", new { id = reservation.Id });
        }
    }
}
