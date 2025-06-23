using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Capstone_project.Models;
using Capstone_project.data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Capstone_project.Controllers
{
    public class HomeController : Controller
    {
        private readonly context _context;

        public HomeController(context context)
        {
            _context = context;
        }

        // ---------------- Display Home Page with Doctors ----------------
        public async Task<IActionResult> Home()
        {
            var doctorData = await (
                from signup in _context.SignUps
                join clinic in _context.AddClinics
                    on (signup.FirstName + " " + signup.LastName) equals clinic.DoctorName
                where signup.DoctorId.StartsWith("doc")
                select new home
                {
                    Name = signup.FirstName + " " + signup.LastName,
                    price = clinic.ConsultationFee,
                    Date = clinic.AvailableDays,
                    Specialty = clinic.Specialty
                }).ToListAsync();

            return View(doctorData);
        }

        // ---------------- Show AddClinic Form ----------------
        [HttpGet]
        public IActionResult AddClinic()
        {
            return View();
        }

        // ---------------- Handle AddClinic Form Submission ----------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddClinic(AddClinic model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // جلب المريض المسجل من السيشن
            var patientId = HttpContext.Session.GetString("UserId"); // تأكد إنك خزنت الـ ID مسبقًا

            if (string.IsNullOrEmpty(patientId))
            {
                ModelState.AddModelError("", "يجب تسجيل الدخول أولاً كمريض.");
                return View(model);
            }

            var patient = await _context.SignUps.FirstOrDefaultAsync(p => p.DoctorId == patientId);
            if (patient == null)
            {
                ModelState.AddModelError("", "المريض غير موجود.");
                return View(model);
            }

            // اختيار أول دكتور موجود
            var doctor = await _context.SignUps
                .FirstOrDefaultAsync(d => d.DoctorId.StartsWith("doc"));

            if (doctor == null)
            {
                ModelState.AddModelError("", "لا يوجد دكتور متاح.");
                return View(model);
            }

            model.DoctorName = doctor.FirstName + " " + doctor.LastName;
            model.PatID = patientId;

            _context.AddClinics.Add(model);

            _context.Homes.Add(new home
            {
                Name = model.DoctorName,
                price = model.ConsultationFee,
                Date = model.AvailableDays,
                Specialty = model.Specialty
            });

            await _context.SaveChangesAsync();
            return RedirectToAction("Home");
        }
    }
}