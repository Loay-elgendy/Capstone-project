using Microsoft.AspNetCore.Mvc;
using Capstone_project.Models;
using Capstone_project.data;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Capstone_project.Controllers
{
    public class PrescriptionFormController : Controller
    {
        private readonly context _context;

        public PrescriptionFormController(context context)
        {
            _context = context;
        }

        // GET: /PrescriptionForm
        [HttpGet]
        public IActionResult PrescriptionForm(int patientId)
        {
            // Pass patientId from URL into the form
            var model = new PrescriptionForm
            {
                PatientId = patientId
            };
            return View(model);
        }
        // GET: /PrescriptionForm/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        // POST: /PrescriptionForm/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PrescriptionForm model)
        {
            if (!ModelState.IsValid)
            {
                return View("PrescriptionForm", model);
            }

            // ✅ Check if email exists in SignUps as a Doctor
            var doctor = await _context.SignUps
                .FirstOrDefaultAsync(u => u.Email.ToLower() == model.Email.ToLower() && u.Role == "Doctor");

            if (doctor == null)
            {
                ModelState.AddModelError("Email", "No doctor found with this email.");
                return View("PrescriptionForm", model);
            }

            // ✅ Assign DoctorId
            model.DoctorId = doctor.UserId;

            // ✅ If PatientId not passed, fallback to latest reservation
            if (model.PatientId == 0)
            {
                // Try to get PatientId from the logged-in user (assuming stored in claims)
                var userIdClaim = User.FindFirst("UserId");
                if (userIdClaim != null)
                {
                    model.PatientId = int.Parse(userIdClaim.Value);
                }
                else
                {
                    // If no logged-in patient, fallback to something else (e.g. latest signup)
                    var latestPatient = await _context.SignUps
                        .Where(u => u.Role == "Patient")
                        .OrderByDescending(u => u.Id)
                        .FirstOrDefaultAsync();

                    if (latestPatient != null)
                    {
                        model.PatientId = latestPatient.Id; // Assign from SignUp table
                    }
                }
            }


            // ✅ Save to DB
            await _context.PrescriptionForms.AddAsync(model);
            await _context.SaveChangesAsync();

            TempData["Message"] = "Prescription saved successfully!";

            // ✅ Redirect with doctorId
            return RedirectToAction("Dashboard", "Home", new { UserId = model.DoctorId });
        }
    }
}
