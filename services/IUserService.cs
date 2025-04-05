using System.Threading.Tasks;
using MyWebAPI.Models;

namespace MyWebAPI.Services
{
    public interface IUserService
    {
        Task<User> CreateUser(RegisterRequest request);
    }
}
