namespace RedeSocial.Models.Post;

public enum UserPostsReturnStatus
{
    FoundPosts = 0,
    InvalidUser = 1,
    NoPostsFound = 2
}

public class UserPostsReturn
{
    public bool Found { get; set; } = false;
    public UserPostsReturnStatus Status { get; set; } = UserPostsReturnStatus.NoPostsFound;
    public string StatusName { get; set; } = UserPostsReturnStatus.NoPostsFound.ToString();
    public List<DataBase.Post> Posts { get; set; } = new List<DataBase.Post>();

}