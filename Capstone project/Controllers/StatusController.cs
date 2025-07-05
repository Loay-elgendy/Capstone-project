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

            var matchedPatient = await _context.SignUps
                .FirstOrDefaultAsync(x => x.FirstName == model.FirstName && x.LastName == model.LastName);

            if (matchedPatient == null)
            {
                ModelState.AddModelError("", "No matching patient found.");
                return View(model);
            }
            else {
                model.PatientId = matchedPatient.DoctorId;
            }


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
