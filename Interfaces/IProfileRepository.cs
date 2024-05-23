using english_learning_server.Models;

namespace english_learning_server.Interfaces
{
    public interface IProfileRepository
    {
        Task<Profile> CreateProfileAsync(Profile profileModel);
    }
}