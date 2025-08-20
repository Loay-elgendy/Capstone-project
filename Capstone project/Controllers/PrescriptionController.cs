using Microsoft.AspNetCore.Mvc;
using Capstone_project.Models;
using Capstone_project.data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Capstone_project.Controllers
{
    public class PrescriptionController : Controller
    {
        private readonly context _context;

        public PrescriptionController(context context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Prescription()
        {
            // Get the latest reservation (no doctor filtering needed)
            var maxReservation = _context.Selects
                .OrderByDescending(r => r.Id)
                .FirstOrDefault();

            if (maxReservation == null)
                return NotFound("No reservations found.");

            // Get the latest prescription form (no DoctorID)
            var latestPrescriptionForm = _context.PrescriptionForms
                .OrderByDescending(p => p.Id)
                .FirstOrDefault();

            // Get all patient statuses (no DoctorID)
            var patientStatus = _context.Status
                .OrderByDescending(s => s.id)
                .ToList();

            var model = new Prescription
            {
                Prescriptionforms = latestPrescriptionForm != null
                    ? new List<PrescriptionForm> { latestPrescriptionForm }
                    : new List<PrescriptionForm>(),
                Prescriptions = patientStatus,
                Reservations = maxReservation != null ? new List<Select> { maxReservation } : new List<Select>()
            };

            return View("Prescription", model);
        }
    }
}
