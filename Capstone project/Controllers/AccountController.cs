﻿using Capstone_project.Models;
using Capstone_project.data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Linq;

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
            if (!ModelState.IsValid) return View(model);

            if (!Regex.IsMatch(model.DoctorId ?? "", @"^(doc|pat)\d+$"))
            {
                ModelState.AddModelError("DoctorId", "Doctor ID must start with 'doc' or 'pat' followed by numbers.");
                return View(model);
            }

            if (await _context.SignUps.AnyAsync(u => u.Email == model.Email))
            {
                ModelState.AddModelError("Email", "This email is already registered.");
                return View(model);
            }

            model.Password = _signUpHasher.HashPassword(model, model.Password);
            _context.SignUps.Add(model);
            await _context.SaveChangesAsync();

            return RedirectToAction("Login");
        }

        // ------------------------ Login ------------------------

        [HttpGet]
        public IActionResult Login()
        {
            ViewBag.SuccessMessage = TempData["Success"]?.ToString();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(Login model)
        {
            if (!ModelState.IsValid) return View(model);

            if (!Regex.IsMatch(model.DoctorId ?? "", @"^(doc|pat)\d+$"))
            {
                ModelState.AddModelError("DoctorId", "Doctor ID must start with 'doc' or 'pat' followed by numbers.");
                return View(model);
            }

            var user = await _context.SignUps
                .FirstOrDefaultAsync(u => u.Email == model.Email && u.DoctorId == model.DoctorId);

            if (user == null ||
                _signUpHasher.VerifyHashedPassword(user, user.Password, model.Password) != PasswordVerificationResult.Success)
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View(model);
            }

            _context.Logins.Add(model);
            await _context.SaveChangesAsync();

            return model.DoctorId.StartsWith("doc")
                ? RedirectToAction("AddClinic", "Home")
                : RedirectToAction("Home", "Home");
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
