using english_learning_server.Models;

namespace english_learning_server.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(User user);
    }
}