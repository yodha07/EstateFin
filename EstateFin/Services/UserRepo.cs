using EstateFin.Data;
using EstateFin.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.EntityFrameworkCore;
using EstateFin.Repositories;
using System.Net.Mail;
using System.Net;

namespace EstateFin.Services
{
    public class UserRepo : IUserRepo
    {
        private readonly ApplicationDbContext db;
        private readonly MailSettings mail;
        public UserRepo(ApplicationDbContext db, MailSettings mail)
        {
            this.db = db;
            this.mail = mail;
        }

        public void Register(User user)
        {
            db.Users.Add(user);
            db.SaveChanges();
        }

        public User? Login(string email, string password)
        {
            return db.Users.FirstOrDefault(u =>
            u.Email == email && u.PasswordHash == password);
        }

        public async Task<User?> LoginWithGoogle(string email)
        {
            return await db.Users.FirstOrDefaultAsync(u => u.Email == email && u.isGoogleUser);
        }

        public async Task<bool> SendEmailAsync(string toEmail, string subject, string body)
        {
            try
            {
                var smtpClient = new SmtpClient(mail.Host, mail.Port)
                {
                    Credentials = new NetworkCredential(mail.UserName, mail.Password),
                    EnableSsl = mail.UseSSL,
                };

                var mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(mail.EmailId, mail.DisplayName);
                mailMessage.To.Add(toEmail);
                mailMessage.Subject = subject;
                mailMessage.Body = body;
                mailMessage.IsBodyHtml = true;

                await smtpClient.SendMailAsync(mailMessage);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
                return false;
            }
        }

        public int GenerateOtp()
        {
            Random random = new Random();
            return random.Next(100000, 999999);
        }

        public void UpdateProfile(User user)
        {
            var existingUser = db.Users.Find(user.UserID);
            if (existingUser != null)
            {
                existingUser.FirstName = user.FirstName;
                existingUser.LastName = user.LastName;
                existingUser.PhoneNumber = user.PhoneNumber;
                existingUser.ProfilePicture = user.ProfilePicture;
                existingUser.Role = user.Role;
                db.SaveChanges();
            }
            else
            {
                throw new Exception("User not found");
            }
        }

        public bool EmailExists(string email)
        {
            return db.Users.Any(u => u.Email == email);
        }

        public User? GetUserByEmail(string email)
        {
            return db.Users.FirstOrDefault(u => u.Email == email);
        }

        public User? GetUserById(int userId)
        {
            return db.Users.FirstOrDefault(u => u.UserID == userId);
        }

    }
}
