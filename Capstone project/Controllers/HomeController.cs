using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Capstone_project.Models;
using Capstone_project.data;
using System.Threading.Tasks;
using System.Linq;

namespace Capstone_project.Controllers
{
    public class HomeController : Controller
    {
        private readonly context _context;

        public HomeController(context context)
        {
            _context = context;
        }

        // ---------------- Display Dashboard ----------------
        [HttpGet]
        [HttpPost]
        public async Task<IActionResult> Dashboard(int id)
        {
            // Fetch reservations for this doctor
            var reservations = await _context.Selects
                .Where(r => r.DoctorId == id) // id is the doctor's UserId from the URL
                .ToListAsync();

            // Fetch all patients
            var patients = await _context.SignUps
                .Where(p => p.Role == "Patient")
                .ToListAsync();

            var model = new Dash
            {
                Reservations = reservations,
                Patients = patients
            };

            ViewBag.Id = id;
            return View(model);
        }

        // ---------------- Show AddClinic Form ----------------
        [HttpGet]
        public IActionResult AddClinic(int userId)
        {
            var model = new AddClinic { UserId = userId };
            return View(model);
        }

        // ---------------- Handle AddClinic POST ----------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddClinic(int userId, AddClinic model)
        {
            if (!ModelState.IsValid)
                return View(model);

            model.UserId = userId; // Ensure the correct userId is set

            _context.AddClinics.Add(model);
            await _context.SaveChangesAsync();

            return RedirectToAction("Dashboard", "Home", new { id = model.UserId });
        }

        // ---------------- Home Page ----------------
        public async Task<IActionResult> Home(int id)
        {
            var clinics = await _context.AddClinics.ToListAsync();

            // Save the logged-in user ID to ViewBag
            ViewBag.UserId = id;

            return View(clinics);
        }


    }
}
