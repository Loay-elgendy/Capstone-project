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

        // GET: /Select/Select/{id}
        [HttpGet]
        public async Task<IActionResult> Select(int id)
        {
            // id comes from previous page (Home) — clinic ID
            var clinic = await _context.AddClinics.FirstOrDefaultAsync(c => c.Id == id);
            if (clinic == null)
            {
                return NotFound();
            }

            ViewBag.ClinicId = id; // pass it to the view if needed
            return View(clinic);
        }

        // POST: /Select/Select/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Select(int id, string selectedTime, string selectedDay)
        {
            var clinic = await _context.AddClinics.FirstOrDefaultAsync(c => c.Id == id);
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
            var user = await _context.SignUps.FirstOrDefaultAsync(u => u.Email == currentUserEmail);

            if (user == null)
                return RedirectToAction("Login", "Account");

            // Create new Select reservation
            var reservation = new Select
            {
                DoctorName = clinic.DoctorName,
                Location = clinic.Location,
                Time = selectedTime,
                Day = selectedDay,
                PatientId = user.Id // link reservation to logged-in patient
            };

            _context.Selects.Add(reservation);
            await _context.SaveChangesAsync();

            // Redirect after successful booking
            return RedirectToAction("Status", "Status", new { id = reservation.Id });
        }
    }
}
