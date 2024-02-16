using Microsoft.Azure.Cosmos;

namespace PMDB;

//TODO EXPLAIN WHY I GET INSTANTIATED AS A SINGLETON
public class MovieCosmosRepository : IMovieRepository
{
    private readonly Container _container;
    public MovieCosmosRepository(string connectionString)
    {
        CosmosClient cosmosClient = new CosmosClient(connectionString);
        Database database = cosmosClient.GetDatabase("movies");
        _container = database.GetContainer("movies");
    }

    public Task<List<Movie>> GetMovies()
    {
        List<Movie> movies = new List<Movie>();
        var iterator = _container.GetItemQueryIterator<Movie>();
        while (iterator.HasMoreResults)
        {
            var response = iterator.ReadNextAsync();
            movies.AddRange(response.Result);
        }
        return Task.FromResult(movies);
    }

    public async Task Add(Movie movie)
    {
        await _container.CreateItemAsync(movie);
    }

    public async void Delete(Guid id)
    {
        await _container.DeleteItemAsync<Movie>(id.ToString(), new PartitionKey(id.ToString()));
    }

    public async Task Update(Movie movie)
    {
        await _container.UpsertItemAsync(movie);
    }

    public async Task<Movie> GetMovie(Guid id)
    {
        return await _container.ReadItemAsync<Movie>(id.ToString(), new PartitionKey(id.ToString()));
    }
}