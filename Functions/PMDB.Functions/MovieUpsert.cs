using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using PMDB.Data;

namespace PMDB.Functions
{
    public class MovieUpsert
    {
        private readonly ILogger<MovieGet> _logger;

        public MovieUpsert(ILogger<MovieGet> logger)
        {
            _logger = logger;
        }

        [Function("MovieUpsert")]
        [QueueOutput("Movie", Connection = "PMDBQueue")]
        public MovieMessage Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "Movie")]
            HttpRequest req, [FromBody]Core.Movie movie)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            if (movie.id != Guid.Empty)
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