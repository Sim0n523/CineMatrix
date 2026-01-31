using CineMatrix.Models;
using CineMatrix.Service.Implementation;
using CineMatrix.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CineMatrix.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMovieService _movieService;
        private readonly IRatingService _ratingService;

        public HomeController(
            IMovieService movieService,
            IRatingService ratingService)
        {
            _movieService = movieService;
            _ratingService = ratingService;
        }

        public IActionResult Index()
        {
            var allMovies = _movieService.GetAllMovies().ToList();
            var watchedMovies = _movieService.GetWatchedMovies().ToList();
            var watchlistMovies = _movieService.GetWatchlist().ToList();
            var allRatings = _ratingService.GetAllRatings().ToList();

            ViewBag.TotalMovies = allMovies.Count;
            ViewBag.WatchedCount = watchedMovies.Count;
            ViewBag.WatchlistCount = watchlistMovies.Count;
            ViewBag.AverageRating = allRatings.Any()
                ? allRatings.Average(r => r.Score)
                : 0;

            ViewBag.RecentlyWatched = watchedMovies
                .OrderByDescending(m => m.WatchedDate)
                .Take(6)
                .ToList();

            ViewBag.RecentlyAdded = allMovies
                .OrderByDescending(m => m.Id)
                .Take(6)
                .ToList();

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }
    }
}
