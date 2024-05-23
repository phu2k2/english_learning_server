using english_learning_server.Data;
using english_learning_server.Interfaces;
using english_learning_server.Models;
using Microsoft.EntityFrameworkCore;

namespace english_learning_server.Repository
{
    public class ProfileRepository : Repository<Profile>, IProfileRepository
    {
        public ProfileRepository(EnglishLearningDbContext context)
            : base(context)
        {

        }

        public async Task<Profile> CreateProfileAsync(Profile profileModel)
        {
            await _context.Profiles.AddAsync(profileModel);
            await _context.SaveChangesAsync();

            return profileModel;
        }
    }
}