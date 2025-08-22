using Capstone_project.data;
using Capstone_project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Capstone_project.Controllers
{
    public class PrescriptionController : Controller
    {
        private readonly context _context;

        public PrescriptionController(context context)
        {
            _context = context;
        }

        public async Task<IActionResult> Prescription(int userId, int doctorId)
        {
            var model = new Prescription
            {
                Prescriptions = await _context.Status
                    .Where(s => s.PatientId == userId)
                    .ToListAsync(),

                Prescriptionforms = await _context.PrescriptionForms
                    .Where(p => p.PatientId == userId && p.DoctorId == doctorId)
                    .ToListAsync(),

                Reservations = await _context.Selects
                    .Where(r => r.DoctorId == doctorId)
                    .ToListAsync()
            };

            return View(model);
        }



    }
}
