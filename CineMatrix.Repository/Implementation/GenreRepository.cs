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
    public class GenreRepository : Repository<Genre>, IGenreRepository
    {
        private readonly ApplicationDbContext _context;

        public GenreRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public Genre? GetByName(string name)
        {
            return Get(
                g => g,
                predicate: g => g.Name == name
            );
        }

        public Genre? GetByTmdbId(int tmdbId)
        {
            return Get(
                g => g,
                predicate: g => g.TmdbId == tmdbId
            );
        }

        public IEnumerable<Genre> GetPopularGenres(int count)
        {
            var genres = GetAll(
                g => g,
                include: q => q.Include(g => g.Movies)
            ).ToList();

            return genres
                .OrderByDescending(g => g.Movies.Count)
                .Take(count);
        }

        public bool ExistsByTmdbId(int tmdbId)
        {
            return _context.Genres.Any(g => g.TmdbId == tmdbId);
        }
    }
}
