using EstateFin.Models;

namespace EstateFin.Repositories
{
    public interface IUserRepo
    {
        void Register(User user);
        User? Login(string email, string password);
        Task<User?> LoginWithGoogle(string email);

    }
}
