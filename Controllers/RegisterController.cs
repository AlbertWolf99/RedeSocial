using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using RedeSocial.DataBase;
using RedeSocial.Models.Register;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace RedeSocial.Controllers;

[ApiController]
[Route("[controller]")]
public class RegisterController : BaseController
{
    private readonly ILogger<RegisterController> _logger;

    public RegisterController(ILogger<RegisterController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Cadastra um novo usuario
    /// </summary>
    [HttpPost(Name = "NewUser")]
    public RegisterReturn Post([FromBody]RegisterRequest req)
    {
        RegisterReturn ret = new RegisterReturn()
        {
            UserName = req.UserName
        };

        if(!DataBase.User.MatchPasswordRequirements(req.Password))
        {
            ret.Status = RegisterReturnStatus.PasswordDoesntMeetRequirements;
            ret.StatusName = ret.Status.ToString();
            ret.Registered = false;
            return ret;
        }

        if(req.UserName.Length < 3 || req.UserName.Length > 49)
        {
            ret.Status = RegisterReturnStatus.InvalidUserName;
            ret.StatusName = ret.Status.ToString();
            ret.Registered = false;
            return ret;
        }

        if(DataBase.User.ExistingUserName(req.UserName))
        {
            ret.Status = RegisterReturnStatus.UserNameAlreadyInUse;
            ret.StatusName = ret.Status.ToString();
            ret.Registered = false;
            List<string> suggestions = new List<string>();
            Random rand = new Random();
            int count = 0;
            while(suggestions.Count < 10 && count < 1000)
            {
                string tmpUserName = $"{req.UserName}{rand.Next(1000, 9999)}";
                if(DataBase.User.ExistingUserName(tmpUserName)) suggestions.Add(tmpUserName);
                count++;
            }
            ret.SuggestedUserName = suggestions;
            return ret;
        }

        byte[] salt = DataBase.User.GenerateSalt();

        var user = new DataBase.User();
        user.UserName = req.UserName.ToLower();
        user.BirthDay = req.Birthday;
        user.Email = req.Email.ToLower();
        user.Password = DataBase.User.HashPassword(salt, req.Password);

        user.Insert();
        
        ret.Status = RegisterReturnStatus.Successful;
        ret.StatusName = ret.Status.ToString();
        ret.Registered = true;

        return ret;
    }
}
