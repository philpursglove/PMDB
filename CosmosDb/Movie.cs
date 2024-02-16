namespace PMDB;

public class Movie
{
    public Guid id { get; set; }
    public string Name { get; set; }
    public string Genre { get; set; }

    public int YearOfRelease { get; set; }
    public string Director { get; set; }
}