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

        [Function("Movie")]
        [HttpGet]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            return new OkObjectResult(_movieRepository.GetMovies());

        }

        [Function("Movie")]
        [HttpGet]
        [Route("/{id}")]
        public IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "Movie/{id}")] HttpRequest req, Guid id)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var movie = _movieRepository.GetMovie(id);
            if (movie == null)
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(movie);

        }

        [Function("Movie")]
        [HttpPost]
        public IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "Movie")]
            HttpRequest req, Movie movie)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");


        }

    }
}
