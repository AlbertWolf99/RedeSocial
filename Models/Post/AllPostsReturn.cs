namespace RedeSocial.Models.Post;

public enum AllPostsReturnStatus
{
    FoundPosts = 0,
    NoPostsFound = 1
}

public class AllPostsReturn
{
    public bool Found { get; set; } = false;
    public AllPostsReturnStatus Status { get; set; } = AllPostsReturnStatus.NoPostsFound;
    public string StatusName { get; set; } = AllPostsReturnStatus.NoPostsFound.ToString();
    public List<DataBase.Post> Posts { get; set; } = new List<DataBase.Post>();
}