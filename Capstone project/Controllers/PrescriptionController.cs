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
        public IActionResult Prescription(string doctorId)
        {
            if (string.IsNullOrEmpty(doctorId))
                return NotFound("Doctor ID is missing.");

            var maxReservation = _context.Selects
                .Where(r => r.DoctorId == doctorId)
                .OrderByDescending(r => r.Id)
                .FirstOrDefault();

            if (maxReservation == null)
                return NotFound("No reservations found for this doctor.");

            var latestPrescriptionForm = _context.PrescriptionForms
                .Where(p => p.DoctorID == doctorId)
                .OrderByDescending(p => p.Id)
                .FirstOrDefault();

            // FIX: Retrieve status by doctor + patient
            var patientStatus = _context.Status
                .Where(s => s.DoctorId == doctorId && s.DoctorId == maxReservation.DoctorId)
                .OrderByDescending(s => s.Id)
                .ToList();

            var model = new Prescription
            {
                Prescriptionforms = latestPrescriptionForm != null
                    ? new List<PrescriptionForm> { latestPrescriptionForm }
                    : new List<PrescriptionForm>(),
                Prescriptions = patientStatus,
                Reservations = new List<Select> { maxReservation }
            };

            return View("Prescription", model);
        }


    }
}
