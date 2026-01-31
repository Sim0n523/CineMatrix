using CineMatrix.Service.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineMatrix.Service.Interface
{
    public interface IRatingService
    {
        RatingDto? GetRatingForMovie(int movieId);
        void AddOrUpdateRating(int movieId, int score, string? review);
        void DeleteRating(int movieId);
        IEnumerable<RatingDto> GetAllRatings();
    }
}
