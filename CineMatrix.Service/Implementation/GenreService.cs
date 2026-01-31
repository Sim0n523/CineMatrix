using CineMatrix.Repository.Interface;
using CineMatrix.Service.DTO;
using CineMatrix.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineMatrix.Service.Implementation
{
    public class GenreService : IGenreService
    {
        private readonly IGenreRepository _genreRepository;

        public GenreService(IGenreRepository genreRepository)
        {
            _genreRepository = genreRepository;
        }

        public IEnumerable<GenreDto> GetAllGenres()
        {
            var genres = _genreRepository.GetAll(g => g).ToList();
            return genres.Select(g => new GenreDto
            {
                Id = g.Id,
                Name = g.Name,
                TmdbId = g.TmdbId
            });
        }

        public GenreDto? GetGenreById(int id)
        {
            var genre = _genreRepository.Get(g => g, predicate: g => g.Id == id);
            if (genre == null) return null;

            return new GenreDto
            {
                Id = genre.Id,
                Name = genre.Name,
                TmdbId = genre.TmdbId
            };
        }
    }
}
