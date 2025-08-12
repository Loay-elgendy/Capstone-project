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
        public async Task<IActionResult> Dashboard(string doctorId)
        {
            if (string.IsNullOrEmpty(doctorId))
                return RedirectToAction("Login", "Account");

            var reservations = await _context.Selects
                .Where(r => r.DoctorId == doctorId)
                .ToListAsync();

            var patients = await _context.SignUps
                .Where(p => p.DoctorId.StartsWith("pat"))
                .ToListAsync();

            var model = new Dash
            {
                Reservations = reservations,
                Patients = patients
            };

            ViewBag.DoctorID = doctorId;
            return View(model);
        }


        // ---------------- Show AddClinic Form ----------------
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

        // ---------------- Handle AddClinic POST ----------------
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

            return RedirectToAction("Dashboard", new { doctorId = model.DoctorID });
        }
        public async Task<IActionResult> Home()
        {
            var clinics = await _context.AddClinics.ToListAsync();
            return View(clinics);
        }
    }
}
