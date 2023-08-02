namespace RedeSocial.Models.Register;


public enum RegisterReturnStatus
{
    Successful = 0,
    PasswordDoesntMeetRequirements = 1,
    UserNameAlreadyInUse = 2,
    InvalidBirthDay = 3,
    InvalidUserName = 4,
    ServiceInMaintence = 5
}

public class RegisterReturn
{
    public bool Registered { get; set; } = false;
    public RegisterReturnStatus Status { get; set; } = RegisterReturnStatus.ServiceInMaintence;
    public string StatusName { get; set; } = RegisterReturnStatus.ServiceInMaintence.ToString();
    public string UserName { get; set; } = "";
    public IEnumerable<String> SuggestedUserName {get; set;} = new List<String>();

}