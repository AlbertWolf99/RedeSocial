namespace RedeSocial.Models.User;

public class UserDataReturn
{
    public bool Found { get; set; } = false;
    public string UserName { get; set; } = "";
    public string Email { get; set; } = "";
    public DateTime BirthDay { get; set; }

}