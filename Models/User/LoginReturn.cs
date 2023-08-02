namespace RedeSocial.Models.User;

public enum LoginReturnStatus
{
    Logged = 0,
    UserNameOrPasswordInvalid = 1,
    BadRequest = 2
}

public class LoginReturn
{
    public LoginReturnStatus Status { get; set; } = LoginReturnStatus.BadRequest;
    public string StatusName { get; set; } = LoginReturnStatus.BadRequest.ToString();
    public bool Logged { get; set; } = false;
    public string Token { get; set; } = "";

}