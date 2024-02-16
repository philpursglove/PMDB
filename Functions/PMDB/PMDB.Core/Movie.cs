namespace PMDB.Core
{
    public class Movie
    {
        public Guid id { get; set; }
        public string Name { get; set; }
        public string Genre { get; set; }

        public int YearOfRelease { get; set; }
        public string Director { get; set; }

        public List<MovieComment> Comments { get; set; }
    }

    public class MovieComment
    {
        public Guid id { get; set; }
        public Guid MovieId { get; set; }
        public string Comment { get; set; }
        public DateTime CommentDate { get; set; }
    }
}
