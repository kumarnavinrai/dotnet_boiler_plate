using System.Threading.Tasks;
using MyWebAPI.Models;

namespace MyWebAPI.Services
{
    public interface ITokenService
    {
        Task<AuthTokens> GenerateAuthTokens(User user);
    }
}
