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
            var clinic = await _context.AddClinics.FirstOrDefaultAsync(c => c.Id == id);
            if (clinic == null)
            {
                return NotFound();
            }
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

            // Create new Select reservation
            var reservation = new Select
            {
                DoctorName = clinic.DoctorName,
                Location = clinic.Location,
                Time = selectedTime,
                Day = selectedDay
            };

            _context.Selects.Add(reservation);
            await _context.SaveChangesAsync();

            // Redirect after successful booking
            return RedirectToAction("Status", "Status");
        }
    }
}
