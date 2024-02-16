namespace PMDB;

public interface IMovieRepository
{
    Task<List<Movie>> GetMovies();
    Task Add(Movie movie);
    void Delete(Guid id);
    Task Update(Movie movie);
    Task<Movie> GetMovie(Guid id);
}