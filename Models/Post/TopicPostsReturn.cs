namespace RedeSocial.Models.Post;

public enum TopicPostsReturnStatus
{
    FoundPosts = 0,
    InvalidTopic = 1,
    NoPostsFound = 2
}

public class TopicPostsReturn
{
    public bool Found { get; set; } = false;
    public TopicPostsReturnStatus Status { get; set; } = TopicPostsReturnStatus.NoPostsFound;
    public string StatusName { get; set; } = TopicPostsReturnStatus.NoPostsFound.ToString();
    public List<DataBase.Post> Posts { get; set; } = new List<DataBase.Post>();
}