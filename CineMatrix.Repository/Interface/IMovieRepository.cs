using CineMatrix.Domain.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineMatrix.Repository.Interface
{
    public interface IMovieRepository : IRepository<Movie>
    {
        Movie? GetByIdWithDetails(int id);
        Movie? GetByTmdbId(int tmdbId);
        IEnumerable<Movie> SearchByTitle(string searchTerm);
        IEnumerable<Movie> GetByGenre(int genreId);
        IEnumerable<Movie> GetPopularMovies(int count);
        IEnumerable<Movie> GetRecentMovies(int count);
        IEnumerable<Movie> GetTopRatedMovies(int count);
        bool ExistsByTmdbId(int tmdbId);
    }
}
