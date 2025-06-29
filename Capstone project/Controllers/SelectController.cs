using Microsoft.AspNetCore.Mvc;
using Capstone_project.data;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Capstone_project.Models;

namespace Capstone_project.Controllers
{
    public class SelectController : Controller
    {
        private readonly context _context;

        public SelectController(context context)
        {
            _context = context;
        }

        // GET: Select/Select?clinicId=1
        public async Task<IActionResult> Select(int clinicId)
        {
            var clinic = await _context.AddClinics
                .FirstOrDefaultAsync(c => c.Id == clinicId);

            if (clinic == null)
            {
                return NotFound();
            }

            return View(clinic); // Pass AddClinic model to the view
        }
    }
}
