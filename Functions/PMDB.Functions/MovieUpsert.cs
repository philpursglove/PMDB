using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using PMDB.Data;

namespace PMDB.Functions
{
    public class MovieUpsert
    {
        private readonly ILogger<MovieGet> _logger;
        private readonly IMovieRepository _movieRepository;

        public MovieUpsert(ILogger<MovieGet> logger, IMovieRepository movieRepository)
        {
            _logger = logger;
            _movieRepository = movieRepository;
        }

        [Function("MovieUpsert")]
        [QueueOutput("MovieList", Connection = "PMDBQueue")]
        public MovieMessage Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "Movie")]
            HttpRequest req, Core.Movie movie)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            if (movie.id != null)
            {
                return new MovieMessage()
                {
                    Id = movie.id,
                    Name = movie.Name,
                    Director = movie.Director,
                    Genre = movie.Genre,
                    YearOfRelease = movie.YearOfRelease
                };
            }
            else
            {
                return new MovieMessage()
                {
                    Name = movie.Name,
                    Director = movie.Director,
                    Genre = movie.Genre,
                    YearOfRelease = movie.YearOfRelease
                };
            }
        }

    }
}