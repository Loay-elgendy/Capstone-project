using Microsoft.AspNetCore.Mvc;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System;

namespace Capstone_project.Controllers
{
    public class ForgetController : Controller
    {
        public IActionResult ForgetPassword()
        {
            return View();
        }

        public IActionResult VerificationCode()
        {
            return View();
        }

        [HttpPost]
        public IActionResult VerificationCode(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                TempData["Error"] = "Please enter a valid email.";
                return RedirectToAction("ForgetPassword");
            }

            // Generate 6-digit random code
            var code = new Random().Next(100000, 999999).ToString();

            try
            {
                var message = new MimeMessage();
                message.From.Add(MailboxAddress.Parse("technotrendsza@gmail.com"));
                message.To.Add(MailboxAddress.Parse(email));
                message.Subject = "Reset Password Verification Code";
                message.Body = new TextPart("plain") { Text = $"Your verification code is: {code}" };

                using (var smtp = new SmtpClient())
                {
                    smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                    smtp.Authenticate("technotrendsza@gmail.com", "olmqbiqlvpceqtpp"); // Use your app password
                    smtp.Send(message);
                    smtp.Disconnect(true);
                }

                // Preserve TempData for next request
                TempData["VerificationCode"] = code;
                TempData["TargetEmail"] = email;
                TempData["Success"] = "Verification code sent successfully!";
            }
            catch (Exception)
            {
                TempData["Error"] = "Failed to send email. Please try again.";
            }

            return RedirectToAction("VerificationCode");
        }

        [HttpPost]
        public IActionResult CheckVerificationCode(string code)
        {
            var storedCode = TempData["VerificationCode"] as string;
            var email = TempData["TargetEmail"] as string;

            if (!string.IsNullOrEmpty(storedCode) && storedCode == code)
            {
                TempData["Email"] = email; // Store email for reset page
                return RedirectToAction("ResetPassword");
            }

            TempData["Error"] = "Invalid verification code.";
            return RedirectToAction("VerificationCode");
        }

        [HttpGet]
        public IActionResult RestPassword()
        {
            return View();
        }
    }
}
