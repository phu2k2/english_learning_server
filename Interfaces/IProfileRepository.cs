using english_learning_server.Models;

namespace english_learning_server.Interfaces
{
    public interface IProfileRepository
    {
        Task<Profile?> GetProfileByUserIdAsync(string id);

        Task<Profile> CreateProfileAsync(Profile profileModel);
    }
}