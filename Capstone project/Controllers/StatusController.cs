using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Capstone_project.Models;
using Capstone_project.data;
using System.Threading.Tasks;

namespace Capstone_project.Controllers
{
    public class StatusController : Controller
    {
        private readonly context _context;

        public StatusController(context context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Status(string doctorId)
        {
            var model = new statusmodel();

            if (!string.IsNullOrEmpty(doctorId))
            {
                model.DoctorId = doctorId;
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Status(statusmodel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // Find patient by PatientId
            var matchedPatient = await _context.SignUps
                .FirstOrDefaultAsync(x => x.DoctorId == model.PatientId); // Assuming DoctorId is used as PatientId

            if (matchedPatient == null)
            {
                ModelState.AddModelError("", "Patient ID not found in the system.");
                return View(model);
            }

            // Check if names match
            if (!string.Equals(matchedPatient.FirstName, model.FirstName, StringComparison.OrdinalIgnoreCase) ||
                !string.Equals(matchedPatient.LastName, model.LastName, StringComparison.OrdinalIgnoreCase))
            {
                ModelState.AddModelError("", "Patient ID exists, but the name does not match our records.");
                return View(model);
            }

            // Check if DoctorId is set
            if (string.IsNullOrEmpty(model.DoctorId))
            {
                ModelState.AddModelError("", "Doctor ID is missing.");
                return View(model);
            }

            _context.Status.Add(model);
            await _context.SaveChangesAsync();

            return RedirectToAction("Home", "Home");
        }

    }
}
