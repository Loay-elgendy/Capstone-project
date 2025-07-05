using Microsoft.AspNetCore.Mvc;
using Capstone_project.data;
using Microsoft.EntityFrameworkCore;
using Capstone_project.Models;
using System.Linq;
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

        // GET: View full submitted data by doctor and patient ID
        public async Task<IActionResult> AddDetails(string doctorId, string patientId)
        {
            if (string.IsNullOrEmpty(doctorId) || string.IsNullOrEmpty(patientId))
            {
                return BadRequest("Doctor ID or Patient ID is missing.");
            }

            var statusData = await _context.Status
                .FirstOrDefaultAsync(s => s.PatientId == patientId);

            if (statusData == null)
            {
                return NotFound("No patient data found for the specified IDs.");
            }

            return View("AddDetails", statusData);
        }
    }
}
