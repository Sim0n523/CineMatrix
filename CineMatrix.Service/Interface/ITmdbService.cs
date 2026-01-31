using CineMatrix.Service.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineMatrix.Service.Interface
{
    public interface ITmdbService
    {
        List<TmdbMovieDto> SearchMovies(string query);
        List<TmdbMovieDto> GetPopularMovies();
        List<TmdbMovieDto> GetTrendingMovies();
        TmdbMovieDto? GetMovieDetails(int tmdbId);
    }
}
