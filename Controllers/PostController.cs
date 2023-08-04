using Microsoft.AspNetCore.Mvc;
using RedeSocial.DataBase;
using RedeSocial.Models.Post;
using Microsoft.AspNetCore.Authorization;

namespace RedeSocial.Controllers;

[ApiController]
[Route("[controller]")]
public class PostController : BaseController
{
    private readonly ILogger<PostController> _logger;
    private IConfiguration _config;

    public PostController(ILogger<PostController> logger, IConfiguration config)
    {
        _logger = logger;
        _config = config;
    }

    /// <summary>
    /// Publica um novo post do usuario logado
    /// </summary>
    [HttpPost("NewPost"), Authorize]
    public PublishPostReturn Post([FromBody] PublishPostRequest req)
    {
        PublishPostReturn ret = new PublishPostReturn()
        {
            Text = req.Text
        };

        if (req.Text.Length < 3 || req.Text.Length > 499)
        {
            ret.Status = PublishPostStatus.InvalidText;
            ret.StatusName = ret.Status.ToString();
            ret.Published = false;
            return ret;
        }

        if (!DataBase.Post.IsValidTopic(req.Topic))
        {
            ret.Status = PublishPostStatus.TopicDoesntExist;
            ret.StatusName = ret.Status.ToString();
            ret.Published = false;
            return ret;
        }

        var post = new DataBase.Post();
        
        post.UserUserName = CurrentUser().UserName;
        post.Text = req.Text;
        post.Topic = req.Topic;
        post.PublishTime = DateTime.Now; 

        post.Insert();
        
        ret.Status = PublishPostStatus.Published;
        ret.StatusName = ret.Status.ToString();
        ret.Published = true;

        return ret;
    }

    /// <summary>
    /// Altera o texto de um post do usuario logado
    /// </summary>
    [HttpPost("EditPost"), Authorize]
    public EditPostReturn Post([FromBody] EditPostRequest req)
    {
        EditPostReturn ret = new();
        Post? post = DataBase.Post.FindPostById(req.PostId);
        if(post == null)
        {
            ret.Status = EditPostStatus.InvalidPostId;
            ret.StatusName = ret.Status.ToString();
            ret.EditedPost = false;
            return ret;
        }

        if(post.UserUserName != CurrentUser().UserName)
        {
            ret.Status = EditPostStatus.DifferentUser;
            ret.StatusName = ret.Status.ToString();
            ret.EditedPost = false;
            return ret;
        }

        if (req.NewText.Length < 3 || req.NewText.Length > 499)
        {
            ret.Status = EditPostStatus.InvalidText;
            ret.StatusName = ret.Status.ToString();
            ret.EditedPost = false;
            return ret;
        }

        ret.Status = EditPostStatus.Edited;
        ret.StatusName = ret.Status.ToString();
        ret.EditedPost = true;
        post.Text = req.NewText;
        post.Update();
        return ret;
    }

    /// <summary>
    /// Busca todos os posts
    /// </summary>
    [HttpGet("GetPosts"), Authorize]
    public AllPostsReturn GetAllPosts()
    {
        AllPostsReturn ret = new();
        List<Post> posts = DataBase.Post.FindAllPosts();
        if(!posts.Any())
        {
            ret.Status = AllPostsReturnStatus.NoPostsFound;
            ret.StatusName = ret.Status.ToString();
            ret.Found = false;
            return ret;
        }

        ret.Status = AllPostsReturnStatus.FoundPosts;
        ret.StatusName = ret.Status.ToString();
        ret.Found = true;
        ret.Posts = posts;
        return ret;
    }

    /// <summary>
    /// Busca os posts de um determinado usuario
    /// </summary>
    [HttpGet("GetUserPosts/{userName}"), Authorize]
    public UserPostsReturn GetUserPosts(string userName)
    {
        UserPostsReturn ret = new();
        if(!DataBase.User.ExistingUserName(userName))
        {
            ret.Status = UserPostsReturnStatus.InvalidUser;
            ret.StatusName = ret.Status.ToString();
            ret.Found = false;
            return ret;
        }

        List<Post> posts = DataBase.Post.FindPostsByUserName(userName);
        if(!posts.Any())
        {
            ret.Status = UserPostsReturnStatus.NoPostsFound;
            ret.StatusName = ret.Status.ToString();
            ret.Found = false;
            return ret;
        }

        ret.Status = UserPostsReturnStatus.FoundPosts;
        ret.StatusName = ret.Status.ToString();
        ret.Found = true;
        ret.Posts = posts;

        return ret;
    }

    /// <summary>
    /// Busca os posts de um determinado tema
    /// </summary>
    [HttpGet("GetTopicPosts/{topic}"), Authorize]
    public TopicPostsReturn GetPostsByTopic(string topic)
    {
        TopicPostsReturn ret = new();
        if(!DataBase.Post.IsValidTopic(topic))
        {
            ret.Status = TopicPostsReturnStatus.InvalidTopic;
            ret.StatusName = ret.Status.ToString();
            ret.Found = false;
            return ret;
        }

        List<Post> posts = DataBase.Post.FindPostsByTopic(topic);
        if(!posts.Any())
        {
            ret.Status = TopicPostsReturnStatus.NoPostsFound;
            ret.StatusName = ret.Status.ToString();
            ret.Found = false;
            return ret;
        }

        ret.Status = TopicPostsReturnStatus.FoundPosts;
        ret.StatusName = ret.Status.ToString();
        ret.Found = true;
        ret.Posts = posts;

        return ret;
    }

}