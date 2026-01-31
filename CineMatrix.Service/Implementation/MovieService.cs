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
    public class MovieService : IMovieService
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IGenreRepository _genreRepository;
        private readonly ITmdbService _tmdbService;

        public MovieService(
            IMovieRepository movieRepository,
            IGenreRepository genreRepository,
            ITmdbService tmdbService)
        {
            _movieRepository = movieRepository;
            _genreRepository = genreRepository;
            _tmdbService = tmdbService;
        }

        public IEnumerable<MovieDto> GetAllMovies()
        {
            var movies = _movieRepository.GetAll(
                m => m,
                include: q => q.Include(m => m.Genres).Include(m => m.Ratings),
                orderBy: q => q.OrderByDescending(m => m.CreatedAt)
            );

            var moviesList = movies.ToList();
            return moviesList.Select(m => MapToDto(m)).ToList();
        }

        public MovieDto? GetMovieById(int id)
        {
            var movie = _movieRepository.Get(
                m => m,
                predicate: m => m.Id == id,
                include: q => q.Include(m => m.Genres)
                    .Include(m => m.Ratings)
            );

            return movie != null ? MapToDto(movie) : null;
        }

        public MovieDto CreateMovie(CreateMovieDto movieDto)
        {
            var movie = new Movie
            {
                Title = movieDto.Title,
                Overview = movieDto.Overview,
                ReleaseDate = movieDto.ReleaseDate,
                PosterPath = movieDto.PosterPath,
                RuntimeMinutes = movieDto.RuntimeMinutes,
                Director = movieDto.Director
            };

            foreach (var genreId in movieDto.GenreIds)
            {
                var genre = _genreRepository.Get(g => g, predicate: g => g.Id == genreId);
                if (genre != null)
                {
                    movie.Genres.Add(genre);
                }
            }

            var created = _movieRepository.Insert(movie);
            return MapToDto(created);
        }

        public MovieDto UpdateMovie(MovieDto movieDto)
        {
            var movie = _movieRepository.Get(
                m => m,
                predicate: m => m.Id == movieDto.Id,
                include: q => q.Include(m => m.Genres)
            );

            if (movie == null)
                throw new Exception("Movie not found");

            movie.Title = movieDto.Title;
            movie.Overview = movieDto.Overview;
            movie.ReleaseDate = movieDto.ReleaseDate;
            movie.PosterPath = movieDto.PosterPath;
            movie.RuntimeMinutes = movieDto.RuntimeMinutes;
            movie.Director = movieDto.Director;

            var updated = _movieRepository.Update(movie);
            return MapToDto(updated);
        }

        public void DeleteMovie(int id)
        {
            var movie = _movieRepository.Get(m => m, predicate: m => m.Id == id);
            if (movie != null)
            {
                _movieRepository.Delete(movie);
            }
        }

        public IEnumerable<MovieDto> SearchMovies(string searchTerm)
        {
            var movies = _movieRepository.SearchByTitle(searchTerm).ToList();
            return movies.Select(MapToDto);
        }

        public IEnumerable<MovieDto> GetPopularMovies(int count)
        {
            var movies = _movieRepository.GetPopularMovies(count).ToList();
            return movies.Select(MapToDto);
        }

        public IEnumerable<MovieDto> GetRecentMovies(int count)
        {
            var movies = _movieRepository.GetRecentMovies(count).ToList();
            return movies.Select(MapToDto);
        }

        public MovieDto? ImportMovieFromTmdb(int tmdbId)
        {
            try
            {
                var existing = _movieRepository.GetByTmdbId(tmdbId);
                if (existing != null)
                {
                    return MapToDto(existing);
                }

                var tmdbMovie = _tmdbService.GetMovieDetails(tmdbId);
                if (tmdbMovie == null)
                {
                    return null;
                }

                DateTime? releaseDate = null;
                if (!string.IsNullOrEmpty(tmdbMovie.ReleaseDate))
                {
                    DateTime.TryParse(tmdbMovie.ReleaseDate, out var parsed);
                    releaseDate = parsed;
                }

                var movie = new Movie
                {
                    Title = tmdbMovie.Title,
                    Overview = tmdbMovie.Overview,
                    ReleaseDate = releaseDate,
                    PosterPath = tmdbMovie.PosterPath,
                    BackdropPath = tmdbMovie.BackdropPath,
                    TmdbRating = tmdbMovie.VoteAverage,
                    TmdbId = tmdbMovie.Id,
                    IsInWatchlist = false,
                    IsWatched = false
                };

                var created = _movieRepository.Insert(movie);
                return MapToDto(created);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public IEnumerable<MovieDto> GetWatchlist()
        {
            var movies = _movieRepository.GetAll(
                m => m,
                predicate: m => m.IsInWatchlist == true,
                include: q => q.Include(m => m.Genres),
                orderBy: q => q.OrderByDescending(m => m.CreatedAt)
            ).ToList();

            return movies.Select(MapToDto).ToList();
        }

        public IEnumerable<MovieDto> GetWatchedMovies()
        {
            var movies = _movieRepository.GetAll(
                m => m,
                predicate: m => m.IsWatched == true,
                include: q => q.Include(m => m.Genres).Include(m => m.Ratings),
                orderBy: q => q.OrderByDescending(m => m.WatchedDate)
            ).ToList();

            return movies.Select(MapToDto).ToList();
        }

        public void AddToWatchlist(int movieId)
        {
            var movie = _movieRepository.Get(m => m, predicate: m => m.Id == movieId);
            if (movie != null)
            {
                movie.IsInWatchlist = true;
                movie.IsWatched = false;
                _movieRepository.Update(movie);
            }
        }

        public void RemoveFromWatchlist(int movieId)
        {
            var movie = _movieRepository.Get(m => m, predicate: m => m.Id == movieId);
            if (movie != null)
            {
                movie.IsInWatchlist = false;
                _movieRepository.Update(movie);
            }
        }

        public void MarkAsWatched(int movieId)
        {
            var movie = _movieRepository.Get(m => m, predicate: m => m.Id == movieId);
            if (movie != null)
            {
                movie.IsWatched = true;
                movie.IsInWatchlist = false;
                movie.WatchedDate = DateTime.UtcNow;
                _movieRepository.Update(movie);
            }
        }

        public void MarkAsUnwatched(int movieId)
        {
            var movie = _movieRepository.Get(m => m, predicate: m => m.Id == movieId);
            if (movie != null)
            {
                movie.IsWatched = false;
                movie.WatchedDate = null;
                _movieRepository.Update(movie);
            }
        }

        private MovieDto MapToDto(Movie movie)
        {
            var rating = movie.Ratings?.FirstOrDefault();

            return new MovieDto
            {
                Id = movie.Id,
                Title = movie.Title,
                Overview = movie.Overview,
                ReleaseDate = movie.ReleaseDate,
                PosterPath = movie.PosterPath,
                BackdropPath = movie.BackdropPath,
                TmdbRating = movie.TmdbRating,
                TmdbId = movie.TmdbId,
                RuntimeMinutes = movie.RuntimeMinutes,
                Director = movie.Director,

                IsInWatchlist = movie.IsInWatchlist,
                IsWatched = movie.IsWatched,
                WatchedDate = movie.WatchedDate,

                UserRating = rating?.Score,
                UserReview = rating?.Review,

                Genres = movie.Genres?.Select(g => new GenreDto
                {
                    Id = g.Id,
                    Name = g.Name,
                    TmdbId = g.TmdbId
                }).ToList() ?? new List<GenreDto>()
            };
        }
    }
}
