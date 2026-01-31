using CineMatrix.Domain.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineMatrix.Repository.Interface
{
    public interface IRatingRepository : IRepository<Rating>
    {
        IEnumerable<Rating> GetAllRatings();
        IEnumerable<Rating> GetMovieRatings(int movieId);
        Rating? GetRatingForMovie(int movieId);
        IEnumerable<Rating> GetTopRatings(int count);
        IEnumerable<Rating> GetRecentRatings(int count);
        double GetAverageRatingForMovie(int movieId);
        bool MovieHasRating(int movieId);
    }
}
