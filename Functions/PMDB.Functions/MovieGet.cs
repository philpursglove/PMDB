using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using PMDB.Data;

namespace PMDB.Functions
{
    public class MovieGet
    {
        private readonly ILogger<MovieGet> _logger;
        private readonly IMovieRepository _movieRepository;

        public MovieGet(ILogger<MovieGet> logger, IMovieRepository movieRepository)
        {
            _logger = logger;
            _movieRepository = movieRepository;
        }

        [Function("MovieGet")]
        [Route("/{id}")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "Movie/{id}")] HttpRequest req, Guid id)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var movie = await _movieRepository.GetMovie(id);
            if (movie == null)
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(movie);

        }

    }
}