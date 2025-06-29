using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Capstone_project.Models;
using Capstone_project.data;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone_project.Controllers
{
    public class HomeController : Controller
    {
        private readonly context _context;

        public HomeController(context context)
        {
            _context = context;
        }

        // ---------------- Display Home Page with Clinics ----------------
        public async Task<IActionResult> Home()
        {
            var clinics = await _context.AddClinics.ToListAsync();
            return View(clinics); // Pass AddClinic list to the view
        }

        // ---------------- Show AddClinic Form ----------------
        [HttpGet]
        public IActionResult AddClinic()
        {
            return View();
        }

        // ---------------- Handle AddClinic Form Submission ----------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddClinic(AddClinic model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // Get first available doctor
            var doctor = await _context.SignUps.FirstOrDefaultAsync(d => d.DoctorId.StartsWith("doc"));
            if (doctor == null)
            {
                ModelState.AddModelError("", "No available doctor.");
                return View(model);
            }


            _context.AddClinics.Add(model);
            await _context.SaveChangesAsync();

            return RedirectToAction("Home");
        }
    }
}
