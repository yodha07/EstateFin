using EstateFin.Data;
using EstateFin.Models;
using EstateFin.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EstateFin.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserRepo repo;
        private readonly MailSettings mail;
        private readonly ApplicationDbContext db;

        public AccountController(IUserRepo repo, ApplicationDbContext db, MailSettings mail)
        {
            this.repo = repo;
            this.db = db;
            this.mail = mail;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {
                var userExists = db.Users.Any(u => u.Email == user.Email);
                if (userExists)
                {
                    ModelState.AddModelError("", "Email already registered.");
                    return View(user);
                }

                repo.Register(user);
                SendEmailAsync(user.Email, user.FirstName + ", Welcome to EstateFin", "You have successfully registered with EstateFin.").Wait();
                TempData["msg"] = "Registration Successful. Please login to continue.";
                return RedirectToAction("Login");
            }
            return View(user);
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("UserRole") != null)
            {
                var userRole = HttpContext.Session.GetString("UserRole");
                switch (userRole)
                {
                    case "Admin":
                        return RedirectToAction("List", "Account");
                    case "Agent":
                        return RedirectToAction("List", "Account");
                    case "Buyer":
                        return RedirectToAction("List", "Account");
                    case "Seller":
                        return RedirectToAction("List", "Account");
                    case "Tenant":
                        return RedirectToAction("List", "Account");
                    default:
                        ModelState.AddModelError("", "Invalid user role.");
                        return View();
                }
            }
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var user = repo.Login(email, password);
            if (user != null)
            {
                HttpContext.Session.SetString("UserRole", user.Role!);
                HttpContext.Session.SetString("UserName", user.FirstName!);
                HttpContext.Session.SetString("Login", user.UserID.ToString()!);


                switch (user.Role)
                {
                    case "Admin":
                        return RedirectToAction("List", "Account");
                    case "Agent":
                        return RedirectToAction("List", "Account");
                    case "Buyer":
                        return RedirectToAction("List", "Account");
                    case "Seller":
                        return RedirectToAction("List", "Account");
                    case "Tenant":
                        return RedirectToAction("List", "Account");
                    default:
                        ModelState.AddModelError("", "Invalid user role.");
                        return View();
                }
            }
            ModelState.AddModelError("", "Invalid login attempt. Please check your credentials.");
            return View();
        }

        public async Task GoogleLogin()
        {
            await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme, new AuthenticationProperties
            {
                RedirectUri = Url.Action("GoogleResponse")
            });
        }

        //public async Task<IActionResult> GoogleResponse()
        //{
        //    var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        //    //var claims = result.Principal?.Identities.FirstOrDefault().Claims.Select(claims => new
        //    //{
        //    //    claims.Issuer,
        //    //    claims.OriginalIssuer,
        //    //    claims.Type,
        //    //    claims.Value
        //    //}); ;

        //    //return Json(claims);

        //    var claims = result.Principal?.Identities.FirstOrDefault()?.Claims;

        //    var email = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        //    var fullname = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

        //    string firstName = "";
        //    string lasttName = "";

        //    if (!string.IsNullOrEmpty(fullname))
        //    {
        //        var parts = fullname.Split(' ', 2);
        //        firstName = parts[0];
        //        lasttName = parts.Length > 1 ? parts[1] : "";
        //    }

        //    var userInfo = new GoogleUserInfo
        //    {
        //        firstName = firstName,
        //        lastName = lasttName,
        //        Email = email
        //    };

        //    return View("GoogleInfo", userInfo);
        //}

        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            var claims = result.Principal?.Identities.FirstOrDefault()?.Claims;

            var email = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var fullname = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

            string firstName = "";
            string lastName = "";

            if (!string.IsNullOrEmpty(fullname))
            {
                var parts = fullname.Split(' ', 2);
                firstName = parts[0];
                lastName = parts.Length > 1 ? parts[1] : "";
            }

            // Check if user exists
            var existingUser = await repo.LoginWithGoogle(email);
            if (existingUser == null)
            {
                var newUser = new User
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email!,
                    Role = "Buyer",
                    isGoogleUser = true,
                    CreatedAt = DateTime.Now
                };

                repo.Register(newUser);
                existingUser = newUser;
                SendEmailAsync(email, fullname + ", Welcome to EstateFin", "You have successfully registered with EstateFin using Google.").Wait();
            }

            HttpContext.Session.SetString("UserRole", existingUser.Role!);
            HttpContext.Session.SetString("UserName", existingUser.FirstName!);

            return RedirectToAction("List", "Account");
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult List(User user)
        {
            var role = HttpContext.Session.GetString("UserRole");
            if (role == null)
            {
                return RedirectToAction("Login");
            }
            var data = db.Users.ToList();
            return View(data);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            if (await repo.SendEmailAsync(toEmail, subject, body))
            {
                Console.WriteLine("Email sent successfully.");
            }
            else
            {
                Console.WriteLine("Failed to send email.");
            }
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ForgotPassword(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                ModelState.AddModelError("", "Email can't be empty.");
                return View();
            }

            var user = db.Users.FirstOrDefault(u => u.Email == email);

            if (user == null)
            {
                ModelState.AddModelError("", "Email not found.");
                return View();
            }

            if (user.isGoogleUser)
            {
                ModelState.AddModelError("", "Google users cannot reset password. Please use Google login.");
                return View();
            }

            int otp = repo.GenerateOtp();
            string subject = "Your OTP for EstateFin";
            string body = $"Your OTP is: {otp}";

            if (repo.SendEmailAsync(email, subject, body).Result)
            {
                TempData["otp"] = otp;
                TempData["email"] = email;
                return RedirectToAction("VerifyOtp");
            }

            ModelState.AddModelError("", "Failed to send OTP. Please try again.");
            return View();
        }

        [HttpGet]
        public IActionResult VerifyOtp()
        {
            TempData.Keep("otp");
            TempData.Keep("email");
            return View();
        }

        [HttpPost]
        public IActionResult VerifyOtp(string otp)
        {
            var actualOtp = TempData["otp"]?.ToString();
            var email = TempData["email"]?.ToString();

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(actualOtp) || string.IsNullOrEmpty(otp))
            {
                ModelState.AddModelError("", "Missing OTP or email.");
                TempData.Keep(); 
                return View();
            }

            if (otp == actualOtp)
            {

                var user = db.Users.FirstOrDefault(u => u.Email == email);

                if (user == null)
                {
                    ModelState.AddModelError("", "User not found.");
                    return View();
                }

                HttpContext.Session.SetString("UserName", user.FirstName ?? "User");
                HttpContext.Session.SetString("UserRole", user.Role ?? "User");

               
                switch (user.Role)
                {
                    case "Admin":
                        return RedirectToAction("AdminDashboard", "Admin");
                    case "Agent":
                        return RedirectToAction("AgentDashboard", "Agent");
                    case "Buyer":
                    case "Tenant":
                    case "Seller":
                        return RedirectToAction("List", "Account");
                    default:
                        return RedirectToAction("Login", "Account");
                }
            }
            else
            {
                ModelState.AddModelError("", "Invalid OTP. Please try again.");
                TempData.Keep(); 
                return View();
            }
        }

        public IActionResult AdminDashboard()
        {
            ViewBag.ActiveUsers = db.Users.Count(u => u.Role != "Admin");
            ViewBag.TotalProperties = db.Properties.Count();
            ViewBag.TotalBookings = db.Bookings.Count();
            ViewBag.TotalTransactions = db.Transactions.Sum(t => t.Amount);
            ViewBag.AgentCount = db.Users.Count(u => u.Role == "Agent");

            return View();
        }



    }
}
