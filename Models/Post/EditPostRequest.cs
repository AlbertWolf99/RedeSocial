namespace RedeSocial.Models.Post;

public class EditPostRequest
{
    public long PostId { get; set; }
    public string NewText { get; set; } = "";
}