namespace PMDB.Functions;

public class CommentMessage
{
    public Guid MovieId { get; set; }
    public string Comment { get; set; }
    public DateTime CommentDate { get; set; }
}