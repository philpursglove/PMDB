using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using PMDB.Data;

namespace PMDB.Functions
{
    public class Movie
    {
        private readonly ILogger<Movie> _logger;
        private readonly IMovieRepository _movieRepository;

        public Movie(ILogger<Movie> logger, IMovieRepository movieRepository)
        {
            _logger = logger;
            _movieRepository = movieRepository;
        }

        [Function("MovieList")]
        [HttpGet]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            return new OkObjectResult(_movieRepository.GetMovies());

        }

        [Function("MovieGet")]
        [HttpGet]
        [Route("/{id}")]
        public IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "Movie/{id}")] HttpRequest req, Guid id)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var movie = _movieRepository.GetMovie(id);
            if (movie == null)
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(movie);

        }

        [Function("MovieAdd")]
        [HttpPost]
        [QueueOutput("Movie", Connection = "PMDBQueue")]
        public MovieMessage Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "Movie")]
            HttpRequest req, Core.Movie movie)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            return new MovieMessage()
            {
                Name = movie.Name,
                Director = movie.Director,
                Genre = movie.Genre,
                YearOfRelease = movie.YearOfRelease
            };


        }

        [Function("MovieUpdate")]
        [HttpPost]
        [QueueOutput("Movie", Connection = "PMDBQueue")]
        public MovieMessage Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "Movie")]
            HttpRequest req, Core.Movie movie)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            return new MovieMessage()
            {
                Id = movie.id,
                Name = movie.Name,
                Director = movie.Director,
                Genre = movie.Genre,
                YearOfRelease = movie.YearOfRelease
            };
        }
    }
}
