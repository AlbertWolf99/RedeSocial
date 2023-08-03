namespace RedeSocial.Models.User;

public enum UserDataReturnStatus
{
    CurrentUser = 0,
    FoundUser = 1,
    UserNotFound = 2
}

public class UserDataReturn
{
    public bool Found { get; set; } = false;
    public UserDataReturnStatus Status { get; set; } = UserDataReturnStatus.UserNotFound;
    public string StatusName { get; set; } = UserDataReturnStatus.UserNotFound.ToString();
    public string UserName { get; set; } = "";
    public string Email { get; set; } = "";
    public DateTime BirthDay { get; set; }

}