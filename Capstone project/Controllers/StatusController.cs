using Microsoft.AspNetCore.Mvc;
using Capstone_project.Models;
using Capstone_project.data;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

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
            return View(new statusmodel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Status(statusmodel model)
        {
            if (string.IsNullOrWhiteSpace(model.Email))
            {
                ModelState.AddModelError("Email", "Email is required.");
                return View(model);
            }

            // Find patient by email
            var patient = await _context.SignUps.FirstOrDefaultAsync(x => x.Email == model.Email);
            if (patient == null)
            {
                ModelState.AddModelError("Email", "No user found with this email.");
                return View(model);
            }

            // Assign PatientId
            model.PatientId = patient.Id;

            // Ensure EF Core generates the identity
            model.id = 0;

            // Save to database
            _context.Status.Add(model);
            await _context.SaveChangesAsync();

            return RedirectToAction("Home", "Home");
        }
    }
}
