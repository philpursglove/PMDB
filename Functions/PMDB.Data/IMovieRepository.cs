using PMDB.Core;

namespace PMDB.Data
{
    public interface IMovieRepository
    {
        List<Movie> GetMovies();
        Task Add(Movie movie);
        void Delete(Guid id);
        Task Update(Movie movie);
        Task<Movie> GetMovie(Guid id);
    }
}