using CineMatrix.Domain.DomainModels;
using CineMatrix.Repository.Interface;
using CineMatrix.Service.DTO;
using CineMatrix.Service.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineMatrix.Service.Implementation
{
    public class RatingService : IRatingService
    {
        private readonly IRatingRepository _ratingRepository;

        public RatingService(IRatingRepository ratingRepository)
        {
            _ratingRepository = ratingRepository;
        }

        public RatingDto? GetRatingForMovie(int movieId)
        {
            var rating = _ratingRepository.Get(
                r => r,
                predicate: r => r.MovieId == movieId,
                include: q => q.Include(r => r.Movie)
            );

            if (rating == null) return null;

            return new RatingDto
            {
                Id = rating.Id,
                MovieId = rating.MovieId,
                MovieTitle = rating.Movie.Title,
                Score = rating.Score,
                Review = rating.Review,
                WatchedDate = rating.WatchedDate
            };
        }

        public void AddOrUpdateRating(int movieId, int score, string? review)
        {
            var existing = _ratingRepository.Get(
                r => r,
                predicate: r => r.MovieId == movieId
            );

            if (existing != null)
            {
                // Update existing rating
                existing.Score = score;
                existing.Review = review;
                existing.WatchedDate = DateTime.UtcNow;
                _ratingRepository.Update(existing);
            }
            else
            {
                // Create new rating
                var rating = new Rating
                {
                    MovieId = movieId,
                    Score = score,
                    Review = review,
                    WatchedDate = DateTime.UtcNow
                };
                _ratingRepository.Insert(rating);
            }
        }

        public void DeleteRating(int movieId)
        {
            var rating = _ratingRepository.Get(r => r, predicate: r => r.MovieId == movieId);
            if (rating != null)
            {
                _ratingRepository.Delete(rating);
            }
        }

        public IEnumerable<RatingDto> GetAllRatings()
        {
            var ratings = _ratingRepository.GetAll(
                r => r,
                include: q => q.Include(r => r.Movie),
                orderBy: q => q.OrderByDescending(r => r.WatchedDate)
            ).ToList();

            return ratings.Select(r => new RatingDto
            {
                Id = r.Id,
                MovieId = r.MovieId,
                MovieTitle = r.Movie.Title,
                Score = r.Score,
                Review = r.Review,
                WatchedDate = r.WatchedDate
            }).ToList();
        }
    }
}
