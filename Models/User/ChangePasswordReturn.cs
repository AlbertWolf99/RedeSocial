namespace RedeSocial.Models.User;


public enum ChangePasswordReturnStatus
{
    Successful = 0,
    IncorrectPassword = 1,
    PasswordDoesntMeetRequirements = 2,
    InvalidUser = 3
}

public class ChangePasswordReturn
{
    
    public bool ChangedPassword { get; set; } = false;
    public ChangePasswordReturnStatus Status { get; set; } = ChangePasswordReturnStatus.InvalidUser;
    public string StatusName { get; set; } = ChangePasswordReturnStatus.InvalidUser.ToString();

}