using english_learning_server.Dtos.Profile;
using english_learning_server.Models;

namespace english_learning_server.Mappers
{
    public static class ProfileMappers
    {
        public static ProfileDto ToProfileDto(this Profile profileModel)
        {
            return new ProfileDto
            {
                Id = profileModel.Id,
                Sex = profileModel.Sex,
                Birthday = profileModel.Birthday,
                Status = profileModel.Status,
            };
        }

        public static UpdateProfileDto ToProfileFromUpdate(this UpdateProfileDto updateProfileDto)
        {
            return new UpdateProfileDto
            {
                Sex = updateProfileDto.Sex,
                Birthday = updateProfileDto.Birthday,
                Status = updateProfileDto.Status,
            };
        }
    }
}
