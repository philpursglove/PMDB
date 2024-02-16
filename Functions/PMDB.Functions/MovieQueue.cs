using Azure.Storage.Queues.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PMDB.Data;

namespace PMDB.Functions
{
    public class MovieQueue
    {
        private readonly ILogger<MovieQueue> _logger;
        private readonly IMovieRepository _movieRepository;

        public MovieQueue(ILogger<MovieQueue> logger, IMovieRepository movieRepository)
        {
            _logger = logger;
            _movieRepository = movieRepository;
        }

        [Function(nameof(MovieQueue))]
        public async Task Run([QueueTrigger("Movie", Connection = "PMDBQueue")] QueueMessage message)
        {
            MovieMessage movieUpdate = JsonConvert.DeserializeObject<MovieMessage>(message.ToString());

            Core.Movie movie;
            if (movieUpdate.Id.HasValue)
            {
                movie = await _movieRepository.GetMovie(movieUpdate.Id.Value);

                movie.Name = movieUpdate.Name;
                movie.Director = movieUpdate.Director;
                movie.Genre = movieUpdate.Genre;
                movie.YearOfRelease = movieUpdate.YearOfRelease;

                await _movieRepository.Update(movie);
            }
            else
            {
                movie = new PMDB.Core.Movie()
                {
                    id = Guid.NewGuid(),
                    Name = movieUpdate.Name,
                    Director = movieUpdate.Director,
                    Genre = movieUpdate.Genre,
                    YearOfRelease = movieUpdate.YearOfRelease
                };
                await _movieRepository.Add(movie);
            }
        }
    }
}
