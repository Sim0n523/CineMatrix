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
    public class MovieRepository : Repository<Movie>, IMovieRepository
    {
        private readonly ApplicationDbContext _context;

        public MovieRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public Movie? GetByIdWithDetails(int id)
        {
            return Get(
                m => m,
                predicate: m => m.Id == id,
                include: q => q
                    .Include(m => m.Genres)
                    .Include(m => m.Ratings)
            );
        }

        public Movie? GetByTmdbId(int tmdbId)
        {
            return Get(
                m => m,
                predicate: m => m.TmdbId == tmdbId,
                include: q => q.Include(m => m.Genres)
            );
        }

        public IEnumerable<Movie> SearchByTitle(string searchTerm)
        {
            return GetAll(
                m => m,
                predicate: m => m.Title.Contains(searchTerm),
                include: q => q.Include(m => m.Genres).Include(m => m.Ratings)
            );
        }

        public IEnumerable<Movie> GetByGenre(int genreId)
        {
            return GetAll(
                m => m,
                predicate: m => m.Genres.Any(g => g.Id == genreId),
                include: q => q.Include(m => m.Genres).Include(m => m.Ratings)
            );
        }

        public IEnumerable<Movie> GetPopularMovies(int count)
        {
            return GetAll(
                m => m,
                include: q => q.Include(m => m.Genres).Include(m => m.Ratings),
                orderBy: q => q.OrderByDescending(m => m.Ratings.Count)
                    .ThenByDescending(m => m.TmdbRating)
            ).Take(count);
        }

        public IEnumerable<Movie> GetRecentMovies(int count)
        {
            return GetAll(
                m => m,
                predicate: m => m.ReleaseDate.HasValue,
                include: q => q.Include(m => m.Genres),
                orderBy: q => q.OrderByDescending(m => m.ReleaseDate)
            ).Take(count);
        }

        public IEnumerable<Movie> GetTopRatedMovies(int count)
        {
            var moviesWithRatings = GetAll(
                m => m,
                predicate: m => m.Ratings.Any(),
                include: q => q.Include(m => m.Genres).Include(m => m.Ratings)
            ).ToList();

            return moviesWithRatings
                .OrderByDescending(m => m.Ratings.First().Score)
                .Take(count);
        }

        public bool ExistsByTmdbId(int tmdbId)
        {
            return _context.Movies.Any(m => m.TmdbId == tmdbId);
        }
    }
}
