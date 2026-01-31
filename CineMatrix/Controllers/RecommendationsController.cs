using CineMatrix.Service.DTO;
using CineMatrix.Service.Implementation;
using CineMatrix.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CineMatrix.Web.Controllers
{
    public class RecommendationsController : Controller
    {
        private readonly IRecommendationService _recommendationService;
        private readonly IMovieService _movieService;

        public RecommendationsController(IRecommendationService recommendationService, IMovieService movieService)
        {
            _recommendationService = recommendationService;
            _movieService = movieService;
        }

        // GET: Recommendations/Index
        public IActionResult Index()
        {
            try
            {
                var recommendations = _recommendationService.GetPersonalizedRecommendations(20)
                                      ?? new List<RecommendationDto>();

                var watchedCount = _movieService.GetWatchedMovies().Count();
                ViewBag.WatchedCount = watchedCount;

                return View(recommendations);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(new List<RecommendationDto>());
            }
        }
    }
}
