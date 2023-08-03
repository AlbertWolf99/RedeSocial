

namespace RedeSocial.DataBase;

public class Post
{
    static DB _db = new DB();
    public long PostId { get; set; }
    public string UserUserName { get; set; } = "";
    public string Text { get; set; } = "";
    public string Topic { get; set; } = "";
    public DateTime PublishTime { get; set; }

    public static string[] PostTopics = 
    {
        "esportes",
        "musica",
        "tecnologia",
        "comida",
        "outros"
    };

    public void Update()
    {
        _db.Update<Post>(this);
        _db.SaveChanges();
    }

    public void Insert()
    {
        _db.Add<Post>(this);
        _db.SaveChanges();
    }

    public void Delete()
    {
        _db.Remove<Post>(this);
        _db.SaveChanges();
    }

    public static bool IsValidTopic(string topic)
    {
        return Array.Exists(PostTopics, element => element == topic);
    }

    public static Post? FindPostById(long postId)
    {
        return (from p in _db.Posts where p.PostId == postId select p).FirstOrDefault();
    }

    public static List<Post>? FindPostsByUserName(string userName)
    {
        return (from p in _db.Posts where p.UserUserName == userName select p).ToList();
    }

    public static List<Post>? FindPostsByTopic(string topic)
    {
        return (from p in _db.Posts where p.Topic == topic select p).ToList();
    }

}