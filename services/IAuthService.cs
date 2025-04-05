using System.Threading.Tasks;
using MyWebAPI.Models;

namespace MyWebAPI.Services
{
    public interface IAuthService
    {
        Task<User> LoginUserWithEmailAndPassword(string email, string password);
    }
}
