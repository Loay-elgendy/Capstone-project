using Microsoft.AspNetCore.Mvc;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System;

namespace Capstone_project.Controllers
{
    public class ForgetController : Controller
    {
        public IActionResult Forgetpassword()
        {
            return View();
        }

        public IActionResult Verificationcode()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Verificationcode(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                TempData["Error"] = "Please enter a valid email.";
                return RedirectToAction("Forgetpassword");
            }

            // Generate 6-digit random code
            var code = new Random().Next(100000, 999999).ToString();

            try
            {
                var message = new MimeMessage();
                message.From.Add(MailboxAddress.Parse("technotrendsza@gmail.com")); // Your email here
                message.To.Add(MailboxAddress.Parse(email)); // Recipient's email here
                message.Subject = "Reset Password Verification Code";
                message.Body = new TextPart("plain")
                {
                    Text = $"Your verification code is: {code}"
                };

                using (var smtp = new SmtpClient())
                {
                    smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                    smtp.Authenticate("technotrendsza@gmail.com", "olmq biql vpce qtpp"); // Your email & app password here
                    smtp.Send(message);
                    smtp.Disconnect(true);
                }

                // Store for future use
                TempData["VerificationCode"] = code;
                TempData["TargetEmail"] = email;
                TempData["Success"] = "Verification code sent successfully!";
            }
            catch (Exception)
            {
                TempData["Error"] = "Failed to send email. Please try again.";
            }

            return RedirectToAction("Verificationcode");
        }

        [HttpPost]
        public IActionResult CheckVerificationCode(string code)
        {
            var storedCode = TempData["VerificationCode"] as string;
            var email = TempData["TargetEmail"] as string;

            if (storedCode == code)
            {
                // Store email for reset page
                TempData["Email"] = email;
                return RedirectToAction("Restpassword");
            }

            TempData["Error"] = "Invalid verification code.";
            return RedirectToAction("Verificationcode");
        }

        public IActionResult Restpassword()
        {
            return View();
        }
    }
}
