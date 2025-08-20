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
        public IActionResult Status()
        {
            return View(new statusmodel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Status(statusmodel model)
        {
            // Get logged-in user's email
            var email = User.Identity?.Name;
            if (string.IsNullOrWhiteSpace(email))
                return View(model);

            // Find patient by email
            var patient = await _context.SignUps.FirstOrDefaultAsync(x => x.Email == email);
            if (patient == null)
                return View(model);

            // Save patient Id
            model.PatientId = patient.Id;

            // Save data
            _context.Status.Add(model);
            await _context.SaveChangesAsync();

            return RedirectToAction("Home", "Home");
        }
    }
}
