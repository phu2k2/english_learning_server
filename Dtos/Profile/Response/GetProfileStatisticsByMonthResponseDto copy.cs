using System.ComponentModel.DataAnnotations;
namespace english_learning_server.Dtos.Profile.Response
{
    public class GetProfileStatisticsByMonthResponseDto
    {
        public IEnumerable<GetProfileStatisticsByMonthResponseItem> months { get; set; } = new List<GetProfileStatisticsByMonthResponseItem>();
    }

    public class GetProfileStatisticsByMonthResponseItem
    {
        [Required]
        public int Month { get; set; }
        [Required]
        public int NumberOfGames { get; set; }
    }
}