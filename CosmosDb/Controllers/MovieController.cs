using Microsoft.AspNetCore.Mvc;

namespace PMDB.Controllers
{
    public class MovieController : Controller
    {
        private readonly IMovieRepository _movieRepository;
        public MovieController(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }

        public async Task<IActionResult> Index()
        {
            List<Movie> movies = await _movieRepository.GetMovies();

            return View(movies);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Movie movie)
        {
            await _movieRepository.Add(movie);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            Movie movie = await _movieRepository.GetMovie(id);
            return View(movie);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Movie movie)
        {
            await _movieRepository.Update(movie);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Details(Guid id)
        {
            Movie movie = await _movieRepository.GetMovie(id);
            return View(movie);
        }
    }
}
