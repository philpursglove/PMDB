using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using PMDB.Data;

namespace PMDB.Functions
{
    public class MovieList
    {
        private readonly ILogger<MovieList> _logger;
        private readonly IMovieRepository _movieRepository;

        public MovieList(ILogger<MovieList> logger, IMovieRepository movieRepository)
        {
            _logger = logger;
            _movieRepository = movieRepository;
        }

        [Function(nameof(MovieList))]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "Movie")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            return new OkObjectResult(_movieRepository.GetMovies());

        }




    }
}
