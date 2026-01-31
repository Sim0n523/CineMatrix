using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineMatrix.Domain.DomainModels
{
    public class Rating : BaseEntity
    {
        public int UserId { get; set; }
        public int MovieId { get; set; }
        public int Score { get; set; }
        public string? Review { get; set; }
        public DateTime WatchedDate { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public virtual Movie Movie { get; set; } = null!;
    }
}
