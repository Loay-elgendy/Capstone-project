using Capstone_project.data;
using Capstone_project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CapstoneAdminSample.Controllers
{
    public class AdminController : Controller
    {
        private readonly context _db;

        public AdminController(context db)
        {
            _db = db;
        }

        // Users Management
        public IActionResult Users(string? roleFilter)
        {
            var q = _db.SignUps.AsQueryable();

            // Apply filter if selected
            if (!string.IsNullOrEmpty(roleFilter))
            {
                if (roleFilter == "Doctor") q = q.Where(x => x.Role == "Doctor");
                else if (roleFilter == "Patient") q = q.Where(x => x.Role == "Patient");
            }

            // Prepare dropdown list for roles
            ViewBag.RoleList = new SelectList(new[]
            {
                new { Value = "", Text = "All" },
                new { Value = "Doctor", Text = "Doctor" },
                new { Value = "Patient", Text = "Patient" }
            }, "Value", "Text", roleFilter);

            return View(q.OrderBy(x => x.Id).ToList());
        }

        [HttpPost]
        public IActionResult DeleteUser(int id)
        {
            var u = _db.SignUps.Find(id);
            if (u != null)
            {
                _db.SignUps.Remove(u);
                _db.SaveChanges();
            }
            return RedirectToAction("Users");
        }

        // Bookings Management
        public IActionResult Bookings()
        {
            var bookings = _db.Selects.OrderBy(b => b.Id).ToList();
            return View(bookings);
        }

        [HttpPost]
        public IActionResult DeleteBooking(int id)
        {
            var b = _db.Selects.Find(id);
            if (b != null)
            {
                _db.Selects.Remove(b);
                _db.SaveChanges();
            }
            return RedirectToAction("Bookings");
        }

        [HttpPost]
        public IActionResult EditBooking(int id, string day, string time)
        {
            var b = _db.Selects.Find(id);
            if (b != null)
            {
                b.Day = day;
                b.Time = time;
                _db.Selects.Update(b);
                _db.SaveChanges();
            }
            return RedirectToAction("Bookings");
        }

        // GET: /Admin/DoctorClinics
        public async Task<IActionResult> DoctorClinics()
        {
            var clinics = await _db.AddClinics.ToListAsync(); // just get all clinics
            return View(clinics); // matches the view model type: IEnumerable<AddClinic>
        }



        // Optional: Add DeleteDoctor functionality
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteDoctor(int id)
        {
            var doctor = await _db.AddClinics.FindAsync(id);
            if (doctor != null)
            {
                _db.AddClinics.Remove(doctor);
                await _db.SaveChangesAsync();
            }
            return RedirectToAction("DoctorClinics"); // Redirect back to the DoctorClinics page
        }

    }
}
