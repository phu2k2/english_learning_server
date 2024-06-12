namespace english_learning_server.Dtos.Profile.Response
{
    public class GetProfileResponseDto
    {
        public Guid Id { get; set; }

        public bool? Sex { get; set; }

        public DateTime? Birthday { get; set; }

        public bool? Status { get; set; }

        public string? AvatarFilePath { get; set; }
    }
}
