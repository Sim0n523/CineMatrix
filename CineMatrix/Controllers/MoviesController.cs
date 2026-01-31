using CineMatrix.Domain.DomainModels;
using CineMatrix.Repository.Interface;
using CineMatrix.Service.DTO;
using CineMatrix.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CineMatrix.Web.Controllers
{
    public class MoviesController : Controller
    {
        private readonly IMovieService _movieService;
        private readonly ITmdbService _tmdbService;
        private readonly IGenreService _genreService;
        private readonly IRatingService _ratingService;

        public MoviesController(
            IMovieService movieService,
            ITmdbService tmdbService,
            IGenreService genreService,
            IRatingService ratingService)
        {
            _movieService = movieService;
            _tmdbService = tmdbService;
            _genreService = genreService;
            _ratingService = ratingService;
        }

        // GET: Movies
        public IActionResult Index()
        {
            var movies = _movieService.GetAllMovies();
            return View(movies);
        }

        // GET: Movies/Watchlist
        public IActionResult Watchlist()
        {
            var movies = _movieService.GetWatchlist();
            return View(movies);
        }

        // GET: Movies/Watched
        public IActionResult Watched()
        {
            var movies = _movieService.GetWatchedMovies();
            return View(movies);
        }

        // GET: Movies/Details/5
        public IActionResult Details(int id)
        {
            var movie = _movieService.GetMovieById(id);
            if (movie == null)
                return NotFound();

            return View(movie);
        }

        // GET: Movies/Create
        public IActionResult Create()
        {
            ViewBag.Genres = _genreService.GetAllGenres().ToList();
            return View();
        }

        // POST: Movies/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateMovieDto movieDto)
        {
            if (ModelState.IsValid)
            {
                _movieService.CreateMovie(movieDto);
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Genres = _genreService.GetAllGenres();
            return View(movieDto);
        }

        // GET: Movies/Edit/5
        public IActionResult Edit(int id)
        {
            var movie = _movieService.GetMovieById(id);
            if (movie == null)
                return NotFound();

            ViewBag.Genres = _genreService.GetAllGenres();
            return View(movie);
        }

        // POST: Movies/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, MovieDto movieDto)
        {
            if (id != movieDto.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                _movieService.UpdateMovie(movieDto);
                return RedirectToAction(nameof(Details), new { id });
            }

            ViewBag.Genres = _genreService.GetAllGenres();
            return View(movieDto);
        }

        // GET: Movies/Delete/5
        public IActionResult Delete(int id)
        {
            var movie = _movieService.GetMovieById(id);
            if (movie == null)
                return NotFound();

            return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _movieService.DeleteMovie(id);
            return RedirectToAction(nameof(Index));
        }

        // GET: Movies/Search
        public IActionResult Search(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return RedirectToAction(nameof(Index));
            }

            try
            {
                var localMovies = _movieService.SearchMovies(query)?.ToList() ?? new List<MovieDto>();
                var tmdbMovies = _tmdbService.SearchMovies(query)?.ToList() ?? new List<TmdbMovieDto>();

                ViewBag.Query = query;
                ViewBag.LocalMovies = localMovies;
                ViewBag.TmdbMovies = tmdbMovies;

                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.Query = query;
                ViewBag.LocalMovies = new List<MovieDto>();
                ViewBag.TmdbMovies = new List<TmdbMovieDto>();
                return View();
            }
        }

        // GET: Movies/Browse
        public IActionResult Browse()
        {
            try
            {
                var popularMovies = _tmdbService.GetPopularMovies();

                ViewBag.PopularMovies = popularMovies ?? new List<TmdbMovieDto>();

                return View();
            }
            catch (Exception ex)
            {
                ViewBag.PopularMovies = new List<TmdbMovieDto>();
                ViewBag.Error = ex.Message;
                return View();
            }
        }

        // POST: Movies/ImportFromTmdb
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ImportFromTmdb(int tmdbId, bool addToWatchlist = false)
        {
            var movie = _movieService.ImportMovieFromTmdb(tmdbId);
            if (movie == null)
                return BadRequest("Failed to import movie");

            if (addToWatchlist)
            {
                _movieService.AddToWatchlist(movie.Id);
                return RedirectToAction(nameof(Watchlist));
            }

            return RedirectToAction(nameof(Details), new { id = movie.Id });
        }

        // POST: Movies/AddToWatchlist
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddToWatchlist(int tmdbId)
        {
            try
            {

                var movie = _movieService.ImportMovieFromTmdb(tmdbId);

                if (movie == null)
                {
                    TempData["Error"] = "Failed to import movie from TMDb.";
                    return RedirectToAction(nameof(Browse));
                }


                _movieService.AddToWatchlist(movie.Id);

                TempData["Success"] = $"{movie.Title} added to watchlist!";

                return RedirectToAction(nameof(Watchlist));
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error: {ex.Message}";
                return RedirectToAction(nameof(Browse));
            }
        }

        // POST: Movies/RemoveFromWatchlist
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RemoveFromWatchlist(int id)
        {
            _movieService.RemoveFromWatchlist(id);
            return RedirectToAction(nameof(Watchlist));
        }

        // POST: Movies/MarkAsWatched
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult MarkAsWatched(int id)
        {
            _movieService.MarkAsWatched(id);
            return RedirectToAction(nameof(Details), new { id });
        }

        // POST: Movies/MarkAsUnwatched
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult MarkAsUnwatched(int id)
        {
            _movieService.MarkAsUnwatched(id);
            return RedirectToAction(nameof(Details), new { id });
        }

        // POST: Movies/RateMovie
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RateMovie(int movieId, int score, string? review)
        {
            try
            {
                var movie = _movieService.GetMovieById(movieId);
                if (movie != null && !movie.IsWatched)
                {
                    _movieService.MarkAsWatched(movieId);
                }

                _ratingService.AddOrUpdateRating(movieId, score, review);

                TempData["Success"] = "Rating saved successfully!";
                return RedirectToAction(nameof(Details), new { id = movieId });
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error saving rating: {ex.Message}";
                return RedirectToAction(nameof(Details), new { id = movieId });
            }
        }

        // POST: Movies/DeleteRating
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteRating(int movieId)
        {
            _ratingService.DeleteRating(movieId);
            return RedirectToAction(nameof(Details), new { id = movieId });
        }

        [HttpPost]
        public IActionResult AddToWatchlistFromDetails(int id)
        {
            try
            {
                _movieService.AddToWatchlist(id);
                TempData["Success"] = "Movie added to watchlist!";
                return RedirectToAction(nameof(Watchlist));
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error: {ex.Message}";
                return RedirectToAction(nameof(Details), new { id });
            }
        }
    }
}
