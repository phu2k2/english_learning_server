using System.ComponentModel.DataAnnotations;

namespace english_learning_server.Dtos.Profile.Query
{
    public class GetProfileStatisticsQueryDto
    {
        [Required]
        public bool IsFilterByMonth { get; set; }
    }
}