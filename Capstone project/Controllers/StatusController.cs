using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Capstone_project.Models;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using Capstone_project.data;
using Microsoft.EntityFrameworkCore;

namespace Capstone_project.Controllers
{
    public class StatusController : Controller
    {
        private readonly IWebHostEnvironment _environment;
        private readonly context _context;

        public StatusController(IWebHostEnvironment environment, context context)
        {
            _environment = environment;
            _context = context;
        }

        // GET: Status
        [HttpGet]
        public IActionResult Status()
        {
            return View();
        }

        // POST: Status
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Status(statusmodel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // Find the patient user by FirstName and LastName in SignUps table
            var matchedUser = await _context.SignUps
                .FirstOrDefaultAsync(x => x.FirstName == model.FirstName && x.LastName == model.LastName);

            if (matchedUser == null)
            {
                ModelState.AddModelError("", "User not found in SignUp table.");
                return View(model);
            }

            // Construct folder name and path based on patient info and DoctorId
            var folderName = $"{model.FirstName}_{model.LastName}_{matchedUser.DoctorId}";
            var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", folderName);

            // Create folder if it doesn't exist
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var filePaths = new List<string>();

            if (model.UploadedFiles != null && model.UploadedFiles.Count > 0)
            {
                foreach (var file in model.UploadedFiles)
                {
                    if (file.Length > 0)
                    {
                        // To avoid overwriting, add a unique prefix to filename
                        var uniqueName = $"{Path.GetFileNameWithoutExtension(file.FileName)}_{Path.GetRandomFileName().Substring(0, 8)}{Path.GetExtension(file.FileName)}";
                        var fullPath = Path.Combine(uploadsFolder, uniqueName);

                        using (var stream = new FileStream(fullPath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        var relativePath = $"/uploads/{folderName}/{uniqueName}";
                        filePaths.Add(relativePath);
                    }
                }

                // Save the comma-separated relative paths of uploaded files in the model property
                model.UploadedFilePaths = string.Join(",", filePaths);
            }

            // Save the model data including UploadedFilePaths to database
            _context.Status.Add(model);
            await _context.SaveChangesAsync();

            return RedirectToAction("PrescriptionForm", "PrescriptionForm");
        }
    }
}
