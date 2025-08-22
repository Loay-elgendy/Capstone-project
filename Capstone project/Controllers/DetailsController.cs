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

        public async Task<IActionResult> AddDetails(int id)
        {
            var statusData = await _context.Status
                .Where(s => s.PatientId == id)
                .OrderByDescending(s => s.id)
                .FirstOrDefaultAsync();

            if (statusData == null)
            {
                return NotFound("No record found with matching Patient ID.");
            }

            // Show the AddDetails page with patient data
            return View(statusData);
        }

    }
}
