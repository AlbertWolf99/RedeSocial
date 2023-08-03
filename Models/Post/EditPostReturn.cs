namespace RedeSocial.Models.Post;


public enum EditPostStatus
{
    Edited = 0,
    InvalidPostId = 1,
    DifferentUser = 2,
    InvalidText = 3,
    ServiceInMaintence = 4
}


public class EditPostReturn
{
    public bool EditedPost { get; set; } = false;
    public EditPostStatus Status { get; set; } = EditPostStatus.ServiceInMaintence;
    public string StatusName { get; set; } = EditPostStatus.ServiceInMaintence.ToString();
}