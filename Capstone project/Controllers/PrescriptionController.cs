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

        // View prescription by DoctorId (string)
        [HttpGet]
        public IActionResult Prescription(string doctorId)
        {
            if (string.IsNullOrEmpty(doctorId))
                return NotFound("Doctor ID is missing.");

            // Get reservations linked to this doctor
            var reservations = _context.Selects
                .Where(r => r.DoctorId == doctorId)
                .ToList();

            if (reservations.Count == 0)
                return NotFound("No reservations found for this doctor.");

            // Get prescription forms by doctor
            var prescriptionForms = _context.PrescriptionForms
                .Where(p => p.DoctorID == doctorId)
                .ToList();

            // Get status models related to those reservations
            var reservationIds = reservations.Select(r => r.Id).ToList();
            var patientStatus = _context.Status
                .Where(s => reservationIds.Contains(s.Id))
                .ToList();

            var model = new Prescription
            {
                Prescriptionforms = prescriptionForms,
                Prescriptions = patientStatus,
                Reservations = reservations
            };

            return View("Prescription", model);
        }
    }
}
