namespace RedeSocial.Models.Post;

public enum PublishPostStatus
{
    Published = 0,
    InvalidText = 1,
    TopicDoesntExist = 2,
    ServiceInMaintence = 3
}

public class PublishPostReturn
{
    public bool Published { get; set; } = false;
    public PublishPostStatus Status { get; set; } = PublishPostStatus.ServiceInMaintence;
    public string StatusName { get; set; } = PublishPostStatus.ServiceInMaintence.ToString();
    public string Text { get; set; } = "";
}