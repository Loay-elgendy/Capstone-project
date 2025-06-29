using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Capstone_project.Models;
using System.IO;
using System.Threading.Tasks;
using System.Linq;

namespace Capstone_project.Controllers
{
    public class StatusController : Controller
    {
        private readonly IWebHostEnvironment _environment;
        private readonly ApplicationDbContext _context;

        public StatusController(IWebHostEnvironment environment, ApplicationDbContext context)
        {
            _environment = environment;
            _context = context;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Status(statusmodel model)
        {
            if (ModelState.IsValid)
            {
                // First save to get Id
                _context.Status.Add(model);
                await _context.SaveChangesAsync();

                var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", model.Id.ToString());
                Directory.CreateDirectory(uploadsFolder);

                var filePaths = new List<string>();

                if (model.UploadedFiles != null && model.UploadedFiles.Count > 0)
                {
                    foreach (var file in model.UploadedFiles)
                    {
                        if (file.Length > 0)
                        {
                            var uniqueName = Path.GetFileName(file.FileName);
                            var fullPath = Path.Combine(uploadsFolder, uniqueName);
                            using (var stream = new FileStream(fullPath, FileMode.Create))
                            {
                                await file.CopyToAsync(stream);
                            }

                            // Save relative path (e.g., /uploads/3/image.png)
                            var relativePath = $"/uploads/{model.Id}/{uniqueName}";
                            filePaths.Add(relativePath);
                        }
                    }

                    // Update record with file paths
                    model.UploadedFilePaths = string.Join(",", filePaths);
                    _context.Status.Update(model);
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction("PrescriptionForm", "PrescriptionForm");
            }

            return View(model);
        }
    }
}
