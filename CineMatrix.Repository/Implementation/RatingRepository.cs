using CineMatrix.Domain.DomainModels;
using CineMatrix.Repository.Data;
using CineMatrix.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineMatrix.Repository.Implementation
{
    public class RatingRepository : Repository<Rating>, IRatingRepository
    {
        private readonly ApplicationDbContext _context;

        public RatingRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public IEnumerable<Rating> GetAllRatings()
        {
            return GetAll(
                r => r,
                include: q => q.Include(r => r.Movie).ThenInclude(m => m.Genres),
                orderBy: q => q.OrderByDescending(r => r.WatchedDate)
            );
        }

        public IEnumerable<Rating> GetMovieRatings(int movieId)
        {
            return GetAll(
                r => r,
                predicate: r => r.MovieId == movieId,
                include: q => q.Include(r => r.Movie),
                orderBy: q => q.OrderByDescending(r => r.WatchedDate)
            );
        }

        public Rating? GetRatingForMovie(int movieId)
        {
            return Get(
                r => r,
                predicate: r => r.MovieId == movieId,
                include: q => q.Include(r => r.Movie)
            );
        }

        public IEnumerable<Rating> GetTopRatings(int count)
        {
            return GetAll(
                r => r,
                include: q => q.Include(r => r.Movie).ThenInclude(m => m.Genres),
                orderBy: q => q.OrderByDescending(r => r.Score)
            ).Take(count);
        }

        public IEnumerable<Rating> GetRecentRatings(int count)
        {
            return GetAll(
                r => r,
                include: q => q.Include(r => r.Movie),
                orderBy: q => q.OrderByDescending(r => r.WatchedDate)
            ).Take(count);
        }

        public double GetAverageRatingForMovie(int movieId)
        {
            var ratings = GetAll(
                r => r.Score,
                predicate: r => r.MovieId == movieId
            ).ToList();

            return ratings.Any() ? ratings.Average() : 0;
        }

        public bool MovieHasRating(int movieId)
        {
            return _context.Ratings.Any(r => r.MovieId == movieId);
        }
    }
}
