using System.Threading.Tasks;
using _144Canvas.API.Models;

namespace _144Canvas.API.Data
{
    public interface IAuthRepository
    {
        Task<User> Register(User user, string Password);
        Task<User> Login(string username, string password);
        Task<bool> UserExists(string username);
        Task<bool> ChangePassword(int id,string oP, string nP);
    }
}