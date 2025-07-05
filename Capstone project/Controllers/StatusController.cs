using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Capstone_project.Models;
using Capstone_project.data;
using System;
using System.Linq;
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

            // Find patient by PatientId (assuming DoctorId is being used as PatientId)
            var matchedPatient = await _context.SignUps
                .FirstOrDefaultAsync(x => x.DoctorId == model.PatientId);

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

            foreach (var prop in typeof(statusmodel).GetProperties())
            {
                if (prop.PropertyType == typeof(string))
                {
                    var value = prop.GetValue(model) as string;
                    if (string.IsNullOrWhiteSpace(value))
                    {
                        prop.SetValue(model, "Not Answered");
                    }
                }
                else if (prop.PropertyType == typeof(int?))
                {
                    var value = prop.GetValue(model) as int?;
                    if (!value.HasValue)
                    {
                        prop.SetValue(model, -1);
                    }
                }
            }


            _context.Status.Add(model);
            await _context.SaveChangesAsync();

            return RedirectToAction("Home", "Home");
        }
    }
}
