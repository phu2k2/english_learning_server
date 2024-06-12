using english_learning_server.Dtos.Profile.Response;
using english_learning_server.Models;

namespace english_learning_server.Mappers
{
    public static class ProfileMappers
    {
        public static GetProfileResponseDto ToProfileResponseDto(this Profile profileModel)
        {
            return new GetProfileResponseDto
            {
                Id = profileModel.Id,
                Sex = profileModel.Sex,
                Birthday = profileModel.Birthday,
                Status = profileModel.Status,
                AvatarFilePath = profileModel.AvatarFilePath
            };
        }
    }
}
