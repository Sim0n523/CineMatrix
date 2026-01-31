using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineMatrix.Domain.DomainModels
{
    public class Movie : BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public string? Overview { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public string? PosterPath { get; set; }
        public string? BackdropPath { get; set; }
        public decimal? TmdbRating { get; set; }
        public int? TmdbVoteCount { get; set; }
        public int? TmdbId { get; set; } // If null = manually created, if set = from TMDb
        public int? RuntimeMinutes { get; set; }
        public string? Director { get; set; }
        public bool IsInWatchlist { get; set; } = false;
        public bool IsWatched { get; set; } = false;
        public DateTime? WatchedDate { get; set; }

        // Computed property
        public double AverageUserRating
        {
            get
            {
                if (Ratings == null || !Ratings.Any())
                    return 0;
                return Math.Round(Ratings.Average(r => r.Score), 1);
            }
        }

        // Navigation properties
        public virtual ICollection<Genre> Genres { get; set; } = new List<Genre>();
        public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();
    }
}
