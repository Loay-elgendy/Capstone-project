using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Capstone_project.Models;
using Capstone_project.data;
using System.Threading.Tasks;
using System.Linq;

namespace Capstone_project.Controllers
{
    public class DetailsController : Controller
    {
        private readonly context _context;

        public DetailsController(context context)
        {
            _context = context;
        }

        // GET: View the latest patient data by PatientId (max Id entry)
        public async Task<IActionResult> AddDetails(string patientId)
        {
            if (string.IsNullOrEmpty(patientId))
            {
                return BadRequest("Patient ID is missing.");
            }

            var statusData = await _context.Status
                .Where(s => s.PatientId.ToString() == patientId )
                .OrderByDescending(s => s.Id)
                .FirstOrDefaultAsync();

            if (statusData == null)
            {
                return NotFound("No record found with matching Patient ID.");
            }
            else
            {
                return View("AddDetails", statusData);
            }
        }
    }
}
