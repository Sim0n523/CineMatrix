using CineMatrix.Domain.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineMatrix.Repository.Interface
{
    public interface IGenreRepository : IRepository<Genre>
    {
        Genre? GetByName(string name);
        Genre? GetByTmdbId(int tmdbId);
        IEnumerable<Genre> GetPopularGenres(int count);
        bool ExistsByTmdbId(int tmdbId);
    }
}
