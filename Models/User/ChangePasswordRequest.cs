namespace RedeSocial.Models.User;


public class ChangePasswordRequest
{
    
    public string NewPassword { get; set; } = "";

    public string OldPassword {get; set; } = "";

}