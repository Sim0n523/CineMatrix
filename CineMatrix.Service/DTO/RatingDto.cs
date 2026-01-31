using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineMatrix.Service.DTO
{
    public class RatingDto
    {
        public int Id { get; set; }
        public int MovieId { get; set; }
        public string MovieTitle { get; set; } = string.Empty;
        public int Score { get; set; }
        public string? Review { get; set; }
        public DateTime WatchedDate { get; set; }
    }

    public class CreateRatingDto
    {
        public int MovieId { get; set; }
        public int Score { get; set; }
        public string? Review { get; set; }
    }
}
