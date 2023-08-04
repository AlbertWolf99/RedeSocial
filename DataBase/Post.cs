

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

    /// <summary>
    /// Verifica se o tema pertence aos possiveis temas de Post.
    /// </summary>
    /// <param name="topic"> nome do tema a ser verificado. </param>
    /// <returns>
    /// True se o tema for encontrado, False caso contrario.
    /// </returns>
    public static bool IsValidTopic(string topic)
    {
        return Array.Exists(PostTopics, element => element == topic);
    }

    /// <summary>
    /// Busca o post com o Id especificado.
    /// </summary>
    /// <param name="postId"> id do post a ser buscado. </param>
    /// <returns>
    /// O post com o id passado, ou null caso não exista.
    /// </returns>
    public static Post? FindPostById(long postId)
    {
        return (from p in _db.Posts where p.PostId == postId select p).FirstOrDefault();
    }

    /// <summary>
    /// Busca todos os posts existentes.
    /// </summary>
    /// <returns>
    /// Uma lista com todos os posts existentes.
    /// </returns>
    public static List<Post> FindAllPosts()
    {
        return (from p in _db.Posts orderby p.PublishTime descending select p).ToList();
    }

    /// <summary>
    /// Busca todos os posts de um determinado usuario.
    /// </summary>
    /// <param name="userName"> nome de usuario de quem publicou os posts a serem buscados. </param>
    /// <returns>
    /// Uma lista com todos os posts do usuario especificado.
    /// </returns>
    public static List<Post> FindPostsByUserName(string userName)
    {
        return (from p in _db.Posts where p.UserUserName == userName orderby p.PublishTime descending select p).ToList();
    }

    /// <summary>
    /// Busca todos os posts de um determinado tema.
    /// </summary>
    /// <param name="topic"> tema dos posts a serem buscados. </param>
    /// <returns>
    /// Uma lista com todos os posts do tema especificado.
    /// </returns>
    public static List<Post> FindPostsByTopic(string topic)
    {
        return (from p in _db.Posts where p.Topic == topic orderby p.PublishTime descending select p).ToList();
    }

}