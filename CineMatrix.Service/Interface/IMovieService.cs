using CineMatrix.Service.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineMatrix.Service.Interface
{
    public interface IMovieService
    {
        // Basic CRUD
        IEnumerable<MovieDto> GetAllMovies();
        MovieDto? GetMovieById(int id);
        MovieDto CreateMovie(CreateMovieDto movieDto);
        MovieDto UpdateMovie(MovieDto movieDto);
        void DeleteMovie(int id);

        // Search & Filter
        IEnumerable<MovieDto> SearchMovies(string searchTerm);
        IEnumerable<MovieDto> GetPopularMovies(int count);
        IEnumerable<MovieDto> GetRecentMovies(int count);

        // TMDb Integration
        MovieDto? ImportMovieFromTmdb(int tmdbId);

        // Watchlist & Watched
        IEnumerable<MovieDto> GetWatchlist();
        IEnumerable<MovieDto> GetWatchedMovies();
        void AddToWatchlist(int movieId);
        void RemoveFromWatchlist(int movieId);
        void MarkAsWatched(int movieId);
        void MarkAsUnwatched(int movieId);
    }
}
