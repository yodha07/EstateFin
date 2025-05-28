using EstateFin.Data;
using EstateFin.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.EntityFrameworkCore;
using EstateFin.Repositories;

namespace EstateFin.Services
{
    public class UserRepo : IUserRepo
    {
        private readonly ApplicationDbContext db;

        public UserRepo(ApplicationDbContext db)
        {
            this.db = db;
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
    }
}
