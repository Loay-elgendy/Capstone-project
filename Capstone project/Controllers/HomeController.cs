using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Capstone_project.Models;
using Capstone_project.data;
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

        // GET: Login page
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string doctorId)
        {
            if (string.IsNullOrEmpty(doctorId))
            {
                ModelState.AddModelError("", "Doctor ID is required.");
                return View();
            }

            // Check if doctor already has a clinic
            var existingClinic = await _context.AddClinics.FirstOrDefaultAsync(c => c.DoctorID == doctorId);

            if (existingClinic != null)
            {
                // Already added clinic -> go to dashboard
                return RedirectToAction("Dashboard", new { doctorId = doctorId });
            }

            // Store the DoctorID in TempData for the AddClinic form
            TempData["DoctorID"] = doctorId;
            return RedirectToAction("AddClinic");
        }

        // GET: Show AddClinic form
        [HttpGet]
        public IActionResult AddClinic()
        {
            var model = new AddClinic();

            if (TempData["DoctorID"] != null)
            {
                model.DoctorID = TempData["DoctorID"].ToString();
                TempData.Keep("DoctorID");
            }

            ViewBag.DoctorID = model.DoctorID;
            return View(model);
        }

        // POST: Handle AddClinic form
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddClinic(AddClinic model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (string.IsNullOrEmpty(model.DoctorID))
            {
                model.DoctorID = TempData["DoctorID"]?.ToString();
            }

            if (string.IsNullOrEmpty(model.DoctorID))
            {
                ModelState.AddModelError("", "Doctor ID is missing.");
                return View(model);
            }

            TempData.Keep("DoctorID");

            _context.AddClinics.Add(model);
            await _context.SaveChangesAsync();

            // Redirect to Dashboard with doctorId in query
            return RedirectToAction("Dashboard", new { doctorId = model.DoctorID });
        }

        // GET: Dashboard
        [HttpGet]
        public IActionResult Dashboard(string doctorId)
        {
            ViewBag.DoctorID = doctorId;
            return View();
        }

        // Display all clinics (Home page)
        public async Task<IActionResult> Home()
        {
            var clinics = await _context.AddClinics.ToListAsync();
            return View(clinics);
        }
    }
}
