using Azure.Storage.Queues.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PMDB.Data;

namespace PMDB.Functions;

public class CommentQueue
{
    private readonly ILogger<CommentQueue> _logger;
    private readonly IMovieRepository _movieRepository;

    public CommentQueue(ILogger<CommentQueue> logger, IMovieRepository movieRepository)
    {
        _logger = logger;
        _movieRepository = movieRepository;
    }

    [Function(nameof(CommentQueue))]
    public async Task Run([QueueTrigger("Movie", Connection = "PMDBQueue")] QueueMessage message)
    {
        CommentMessage commentMessage = JsonConvert.DeserializeObject<CommentMessage>(message.ToString());

        Core.Movie movie = await _movieRepository.GetMovie(commentMessage.MovieId);
        if (movie != null)
        {
            movie.Comments.Add(new Core.MovieComment()
            {
                id = Guid.NewGuid(),
                MovieId = commentMessage.MovieId,
                Comment = commentMessage.Comment,
                CommentDate = commentMessage.CommentDate
            });

            await _movieRepository.Update(movie);
        }
    }
}