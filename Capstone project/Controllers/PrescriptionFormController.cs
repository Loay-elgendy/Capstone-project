using Microsoft.AspNetCore.Mvc;
using Capstone_project.Models;
using Capstone_project.data;
using System.Threading.Tasks;

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
        public IActionResult PrescriptionForm()
        {
            return View(); // Uses Views/PrescriptionForm/PrescriptionForm.cshtml
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

            // Remove DoctorID validation as it's no longer required
            if (string.IsNullOrWhiteSpace(model.Diagnosis) || string.IsNullOrWhiteSpace(model.Medication)
                || string.IsNullOrWhiteSpace(model.Dosage) || string.IsNullOrWhiteSpace(model.Tests))
            {
                ModelState.AddModelError(string.Empty, "All required fields must be filled.");
                return View("PrescriptionForm", model);
            }

            await _context.PrescriptionForms.AddAsync(model);
            await _context.SaveChangesAsync();

            TempData["Message"] = "Prescription saved successfully!";
            return RedirectToAction("Dashboard", "Home");
        }
    }
}
