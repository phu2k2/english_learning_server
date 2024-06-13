using System.ComponentModel.DataAnnotations;
namespace english_learning_server.Dtos.Profile.Response
{
    public class GetProfileStatisticsByDayResponseDto
    {
        public IEnumerable<GetProfileStatisticsByDayResponseItem> days { get; set; } = new List<GetProfileStatisticsByDayResponseItem>();
    }

    public class GetProfileStatisticsByDayResponseItem
    {
        [Required]
        public DateTime Day { get; set; }
        [Required]
        public int NumberOfGames { get; set; }
    }
}