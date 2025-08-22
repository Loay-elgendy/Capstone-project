using Capstone_project.Models;
using Capstone_project.data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace Capstone_project.Controllers
{
    public class AccountController : Controller
    {
        private readonly context _context;
        private readonly IPasswordHasher<SignUp> _signUpHasher;

        public AccountController(context context, IPasswordHasher<SignUp> signUpHasher)
        {
            _context = context;
            _signUpHasher = signUpHasher;
        }

        // ------------------------ SignUp ------------------------
        [HttpGet]
        public IActionResult SignUp() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp(SignUp model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // Validate FirstName & LastName
            if (string.IsNullOrWhiteSpace(model.FirstName))
                ModelState.AddModelError("FirstName", "First name is required.");
            if (string.IsNullOrWhiteSpace(model.LastName))
                ModelState.AddModelError("LastName", "Last name is required.");

            // Validate Role
            if (string.IsNullOrWhiteSpace(model.Role))
                ModelState.AddModelError("Role", "Please select a role.");

            // Validate email
            if (string.IsNullOrWhiteSpace(model.Email))
                ModelState.AddModelError("Email", "Email is required.");
            else if (!new EmailAddressAttribute().IsValid(model.Email))
                ModelState.AddModelError("Email", "Invalid email address.");
            else if (!model.Email.ToLower().EndsWith("@gmail.com"))
                ModelState.AddModelError("Email", "Email must be a Gmail address (@gmail.com).");
            else if (await _context.SignUps.AnyAsync(u => u.Email == model.Email))
                ModelState.AddModelError("Email", "This email is already registered.");

            // Prevent using Admin email
            if (model.Email?.ToLower() == "admin@gmail.com")
                ModelState.AddModelError("Email", "This email is reserved for admin login only.");

            // Validate password
            if (string.IsNullOrWhiteSpace(model.Password))
                ModelState.AddModelError("Password", "Password is required.");
            else if (model.Password.Length < 8)
                ModelState.AddModelError("Password", "Password must be at least 8 characters long.");
            else if (!System.Text.RegularExpressions.Regex.IsMatch(model.Password, @"[A-Z]"))
                ModelState.AddModelError("Password", "Password must contain at least one uppercase letter.");
            else if (!System.Text.RegularExpressions.Regex.IsMatch(model.Password, @"[0-9]"))
                ModelState.AddModelError("Password", "Password must contain at least one number.");
            else if (!System.Text.RegularExpressions.Regex.IsMatch(model.Password, @"[\W_]"))
                ModelState.AddModelError("Password", "Password must contain at least one special character.");

            // Read ConfirmPassword from form
            var confirmPassword = Request.Form["ConfirmPassword"].ToString();
            if (model.Password != confirmPassword)
                ModelState.AddModelError("ConfirmPassword", "Passwords do not match.");

            if (!ModelState.IsValid)
                return View(model);

            // Hash password and save
            model.Password = _signUpHasher.HashPassword(model, model.Password);
            _context.SignUps.Add(model);
            await _context.SaveChangesAsync();

            // Set UserId to the generated Id (primary key)
            model.UserId = model.Id;
            _context.SignUps.Update(model);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Account created successfully. You can now login.";
            return RedirectToAction("Login");
        }




        // ------------------------ Login ------------------------
        [HttpGet]
        public IActionResult Login()
        {
            return View(new Login());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(Login model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // Special case: Admin login
            if (model.Email.ToLower() == "admin@admin.com" && model.Password == "admin1234")
            {
                return RedirectToAction("Users", "Admin");
            }

            // Case-insensitive email search
            var user = await _context.SignUps.FirstOrDefaultAsync(u => u.Email.ToLower() == model.Email.ToLower());

            if (user == null ||
                _signUpHasher.VerifyHashedPassword(user, user.Password, model.Password) != PasswordVerificationResult.Success)
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View(model);
            }


            // Save the login attempt
            _context.Logins.Add(model);
            await _context.SaveChangesAsync();

            if (string.Equals(user.Role, "doctor", StringComparison.OrdinalIgnoreCase))
            {
                var existingClinic = await _context.AddClinics
                    .FirstOrDefaultAsync(c => c.UserId == user.UserId);

                if (existingClinic != null)
                {
                    return RedirectToAction("Dashboard", "Home", new { id = user.UserId });
                }

                // Redirect to AddClinic with correct userId in URL
                return RedirectToAction("AddClinic", "Home", new { userId = user.UserId });
            }

            return RedirectToAction("Home", "Home", new { id = user.UserId });
        }


        // ------------------------ Reset Password ------------------------
        [HttpGet]
        public IActionResult ResetPassword() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(string email, string newPassword, string confirmPassword)
        {
            if (string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(newPassword) ||
                string.IsNullOrWhiteSpace(confirmPassword))
            {
                ModelState.AddModelError(string.Empty, "All fields are required.");
                return View();
            }

            if (newPassword != confirmPassword)
            {
                ModelState.AddModelError(string.Empty, "Passwords do not match.");
                return View();
            }

            if (newPassword.Length < 8)
            {
                ModelState.AddModelError(string.Empty, "Password must be at least 8 characters long.");
                return View();
            }

            var user = await _context.SignUps.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "No account found with this email.");
                return View();
            }

            user.Password = _signUpHasher.HashPassword(user, newPassword);
            _context.SignUps.Update(user);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Password reset successful. You can now login.";
            return RedirectToAction("Login");
        }
    }
}
