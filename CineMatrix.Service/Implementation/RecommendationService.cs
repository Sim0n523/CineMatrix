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
    public class RecommendationService : IRecommendationService
    {
        private readonly IMovieRepository _movieRepository;
        private readonly ITmdbService _tmdbService;

        public RecommendationService(IMovieRepository movieRepository, ITmdbService tmdbService)
        {
            _movieRepository = movieRepository;
            _tmdbService = tmdbService;
        }

        public List<RecommendationDto> GetPersonalizedRecommendations(int count = 20)
        {
            try
            {
                var watchedMovies = _movieRepository.GetAll(
                    m => m,
                    predicate: m => m.IsWatched == true,
                    include: q => q.Include(m => m.Genres).Include(m => m.Ratings)
                ).ToList();

                if (!watchedMovies.Any())
                {
                    return new List<RecommendationDto>();
                }

                var favoriteGenreIds = watchedMovies
                    .Where(m => m.TmdbId.HasValue)
                    .SelectMany(m => m.Genres)
                    .Where(g => g.TmdbId.HasValue)
                    .GroupBy(g => g.TmdbId.Value)
                    .OrderByDescending(g => g.Count())
                    .Take(3)
                    .Select(g => g.Key)
                    .ToList();

                var existingTmdbIds = _movieRepository.GetAll(
                    m => m.TmdbId,
                    predicate: m => m.TmdbId.HasValue
                ).Select(id => id.Value).ToList();

                var allTmdbMovies = new List<TmdbMovieDto>();

                // 1. Get popular movies
                var popular = _tmdbService.GetPopularMovies();
                allTmdbMovies.AddRange(popular);

                // 2. Get trending movies
                var trending = _tmdbService.GetTrendingMovies();
                allTmdbMovies.AddRange(trending);

                // 3. Search for movies based on favorite genres 
                var highlyRatedWatched = watchedMovies
                    .Where(m => m.Ratings != null && m.Ratings.Any() && m.Ratings.First().Score >= 7)
                    .Take(3)
                    .ToList();

                foreach (var movie in highlyRatedWatched)
                {
                    if (!string.IsNullOrEmpty(movie.Title))
                    {
                        // Search for movies with similar titles (first word)
                        var firstWord = movie.Title.Split(' ').FirstOrDefault();
                        if (!string.IsNullOrEmpty(firstWord) && firstWord.Length > 3)
                        {
                            var searchResults = _tmdbService.SearchMovies(firstWord);
                            allTmdbMovies.AddRange(searchResults);
                        }
                    }
                }

                var uniqueMovies = allTmdbMovies
                    .GroupBy(m => m.Id)
                    .Select(g => g.First())
                    .Where(m => !existingTmdbIds.Contains(m.Id))
                    .ToList();


                var recommendations = new List<RecommendationDto>();

                foreach (var tmdbMovie in uniqueMovies)
                {
                    double score = 0;
                    var reasons = new List<string>();

                    if (tmdbMovie.VoteAverage >= 7.5m)
                    {
                        score += (double)tmdbMovie.VoteAverage * 3;
                        reasons.Add($"Highly rated ({tmdbMovie.VoteAverage:0.0}/10)");
                    }
                    else if (tmdbMovie.VoteAverage >= 6.0m)
                    {
                        score += (double)tmdbMovie.VoteAverage * 1.5;
                    }
                    else if (tmdbMovie.VoteAverage > 0)
                    {
                        score += (double)tmdbMovie.VoteAverage;
                    }

                    // Genre matching
                    if (favoriteGenreIds.Any() && tmdbMovie.GenreIds != null && tmdbMovie.GenreIds.Any())
                    {
                        var matchCount = tmdbMovie.GenreIds.Count(gid => favoriteGenreIds.Contains(gid));
                        if (matchCount > 0)
                        {
                            score += matchCount * 20;
                            reasons.Add($"{matchCount} favorite genre(s)");
                        }
                    }

                    // Recent releases
                    if (tmdbMovie.ReleaseDate != null &&
                        DateTime.TryParse(tmdbMovie.ReleaseDate, out var releaseDate))
                    {
                        var yearDiff = DateTime.Now.Year - releaseDate.Year;
                        if (yearDiff <= 1)
                        {
                            score += 3; 
                            reasons.Add("Recent release");
                        }
                        else if (yearDiff <= 5)
                        {
                            score += 1; 
                        }               
                    }

                    if (score > 5) 
                    {
                        recommendations.Add(new RecommendationDto
                        {
                            Movie = MapTmdbToMovieDto(tmdbMovie),
                            Score = score,
                            Reason = reasons.Any() ? string.Join(" • ", reasons) : "Based on your preferences"
                        });
                    }
                }

                

                return recommendations
                    .OrderByDescending(r => r.Score)
                    .Take(count)
                    .ToList();
            }
            catch (Exception ex)
            {
                return new List<RecommendationDto>();
            }
        }

        private MovieDto MapTmdbToMovieDto(TmdbMovieDto tmdbMovie)
        {
            DateTime? releaseDate = null;
            if (!string.IsNullOrEmpty(tmdbMovie.ReleaseDate))
            {
                DateTime.TryParse(tmdbMovie.ReleaseDate, out var parsed);
                releaseDate = parsed;
            }

            return new MovieDto
            {
                Id = 0,
                Title = tmdbMovie.Title,
                Overview = tmdbMovie.Overview,
                ReleaseDate = releaseDate,
                PosterPath = tmdbMovie.PosterPath,
                BackdropPath = tmdbMovie.BackdropPath,
                TmdbRating = tmdbMovie.VoteAverage,
                TmdbId = tmdbMovie.Id,
                IsInWatchlist = false,
                IsWatched = false,
                Genres = new List<GenreDto>()
            };
        }
    }
}
