using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineMatrix.Service.DTO
{
    public class MovieDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Overview { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public string? PosterPath { get; set; }
        public string? BackdropPath { get; set; }
        public decimal? TmdbRating { get; set; }
        public int? TmdbId { get; set; }
        public int? RuntimeMinutes { get; set; }
        public string? Director { get; set; }

        
        public bool IsInWatchlist { get; set; }
        public bool IsWatched { get; set; }
        public DateTime? WatchedDate { get; set; }

        
        public int? UserRating { get; set; }  
        public string? UserReview { get; set; } 

        public List<GenreDto> Genres { get; set; } = new();
    }

    public class CreateMovieDto
    {
        public string Title { get; set; } = string.Empty;
        public string? Overview { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public string? PosterPath { get; set; }
        public int? RuntimeMinutes { get; set; }
        public string? Director { get; set; }
        public List<int> GenreIds { get; set; } = new();
    }
}
