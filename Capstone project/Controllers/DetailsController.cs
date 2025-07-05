using Microsoft.AspNetCore.Mvc;
using Capstone_project.data;
using Microsoft.EntityFrameworkCore;
using Capstone_project.Models;
using System.Threading.Tasks;

namespace Capstone_project.Controllers
{
    public class DetailsController : Controller
    {
        private readonly context _context;

        public DetailsController(context context)
        {
            _context = context;
        }

        // GET: View patient data based on DoctorId + PatientId
        public async Task<IActionResult> AddDetails(string patientId)
        {
            if (string.IsNullOrEmpty(patientId))
            {
                return BadRequest("Patient ID is missing.");
            }

            var statusData = await _context.Status
                .FirstOrDefaultAsync(s => s.PatientId == patientId && s.DoctorId != null);

            if (statusData == null)
            {
                return NotFound("No patient data found for the specified Doctor and Patient IDs.");
            }

            return View("AddDetails", statusData);
        }
    }
}
