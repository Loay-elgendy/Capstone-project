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
            // Fetch reservation by Select.Id (previously DoctorID)
            var reservations = await _context.Selects
                .Where(r => r.Id == id)
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
        public IActionResult AddClinic()
        {
            var model = new AddClinic();
            return View(model);
        }

        // ---------------- Handle AddClinic POST ----------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddClinic(AddClinic model)
        {
            if (!ModelState.IsValid)
                return View(model);

            _context.AddClinics.Add(model);
            await _context.SaveChangesAsync();

            return RedirectToAction("Dashboard", new { id = model.Id });
        }

        // ---------------- Home Page ----------------
        public async Task<IActionResult> Home(int id)
        {
            // id comes from the previous page (Dashboard, AddClinic, etc.)

            // Fetch all clinics (or filter if you want by user ID)
            var clinics = await _context.AddClinics.ToListAsync();

            ViewBag.UserId = id; // pass the ID to the view if needed
            return View(clinics);
        }

    }
}
