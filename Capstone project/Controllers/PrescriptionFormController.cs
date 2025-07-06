using Microsoft.AspNetCore.Mvc;
using Capstone_project.Models;
using Capstone_project.data;

namespace Capstone_project.Controllers
{
    public class PrescriptionFormController : Controller
    {
        private readonly context _context;

        public PrescriptionFormController(context context)
        {
            _context = context;
        }

        public IActionResult PrescriptionForm()
        {
            return View(); // Requires Views/Prescription/Index.cshtml
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
        public IActionResult Create(PrescriptionForm model)
        {
            if (ModelState.IsValid)
            {
                _context.PrescriptionForms.Add(model);
                _context.SaveChanges();

                TempData["Message"] = "Prescription saved!";
                return RedirectToAction("Dashboard", "Home"); 
            }

            return View("PrescriptionForm", model);
        }
    }
}
