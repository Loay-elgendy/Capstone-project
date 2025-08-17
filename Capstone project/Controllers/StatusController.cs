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
        public IActionResult Status()
        {
            var model = new statusmodel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Status(statusmodel model)
        {
            // Fetch logged-in patient securely
            var currentUserEmail = User.Identity.Name;
            var matchedPatient = await _context.SignUps
                .FirstOrDefaultAsync(x => x.Email == currentUserEmail);

            if (matchedPatient == null)
            {
                ModelState.AddModelError("", "User not found.");
                return View(model);
            }

            if (string.IsNullOrEmpty(matchedPatient.Role) ||
                !matchedPatient.Role.Equals("patient", StringComparison.OrdinalIgnoreCase))
            {
                ModelState.AddModelError("", "Only patients can submit a status.");
                return View(model);
            }

            // Auto-fill required fields before validation
            model.PatientId = matchedPatient.Id;
            model.FirstName = matchedPatient.FirstName;
            model.LastName = matchedPatient.LastName;


            // Now validate the model
            if (!ModelState.IsValid)
                return View(model);

            // Fill other empty string and int? fields
            foreach (var prop in typeof(statusmodel).GetProperties())
            {
                if (prop.PropertyType == typeof(string))
                {
                    var value = prop.GetValue(model) as string;
                    if (string.IsNullOrWhiteSpace(value))
                        prop.SetValue(model, "Not Answered");
                }
                else if (prop.PropertyType == typeof(int?))
                {
                    var value = prop.GetValue(model) as int?;
                    if (!value.HasValue)
                        prop.SetValue(model, -1);
                }
            }

            _context.Status.Add(model);
            await _context.SaveChangesAsync();

            return RedirectToAction("Home", "Home");
        }
    }
}
