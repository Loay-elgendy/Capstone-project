using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Capstone_project.Models;
using Capstone_project.data;
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

        // GET: View patient data by primary key (Id) and PatientId
        public async Task<IActionResult> AddDetails(string patientId, int id)
        {
            if (string.IsNullOrEmpty(patientId))
            {
                return BadRequest("Patient ID is missing.");
            }

            var statusData = await _context.Status
                .FirstOrDefaultAsync(s => s.PatientId == patientId && !string.IsNullOrEmpty(s.DoctorId) && s.Id == id);

            if (statusData == null)
            {
                return NotFound("No record found with matching ID and Patient ID.");
            }

            return View("AddDetails", statusData);
        }
    }
}
