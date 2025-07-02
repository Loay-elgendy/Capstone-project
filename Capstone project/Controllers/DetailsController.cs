using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Capstone_project.data;
using Capstone_project.Models;

namespace Capstone_project.Controllers
{
    public class DetailsController : Controller
    {
        private readonly context _context;

        public DetailsController(context context)
        {
            _context = context;
        }

        // Combined Patients + Reservations for Doctor Dashboard
        public async Task<IActionResult> Patients()
        {
            // Fetch patients where DoctorId starts with "pat"
            var patients = await _context.SignUps
                .Where(p => p.DoctorId.StartsWith("pat"))
                .ToListAsync();

            // Fetch all reservations
            var reservations = await _context.Selects.ToListAsync();

            // Defensive: initialize empty lists if null
            patients ??= new List<SignUp>();
            reservations ??= new List<Select>();

            var dashModel = new Dash
            {
                Patients = patients,
                Reservations = reservations
            };

            return View("Dashboard", dashModel);
        }

        // Optional: open AddDetails form for a specific patient
        public IActionResult AddDetails(string id)
        {
            ViewBag.PatientId = id;
            return View();
        }
    }
}
