using Microsoft.AspNetCore.Mvc;
using RedeSocial.DataBase;
using RedeSocial.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace RedeSocial.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : BaseController
{
    private readonly ILogger<UserController> _logger;
    private IConfiguration _config;

    public UserController(ILogger<UserController> logger, IConfiguration config)
    {
        _logger = logger;
        _config = config;
    }

    /// <summary>
    /// Realiza o login de um usuario e retorna um token de autenticação
    /// </summary>
    [HttpPost("Login")]
    public LoginReturn PostLogin([FromBody]LoginRequest req)
    {
        LoginReturn ret = new ();
        if(req == null)
        {
            ret.Status = LoginReturnStatus.BadRequest;
            ret.StatusName = ret.Status.ToString();
            ret.Logged = false;
            return ret;
        }

        if(DataBase.User.Authenticate(req.UserName, req.Password))
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"] ?? ""));
            var issuer = _config["Jwt:Issuer"];
            var audience = _config["Jwt:Audience"];

            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, req.UserName.ToString()),
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = signinCredentials
            };

            
	        var token = tokenHandler.CreateToken(tokenDescriptor);
            ret.Token = tokenHandler.WriteToken(token);
            ret.Logged = true;
            ret.Status = LoginReturnStatus.Logged;
            ret.StatusName = ret.Status.ToString();
        }
        else
        {
            ret.Logged = false;
            ret.Status = LoginReturnStatus.UserNameOrPasswordInvalid;
            ret.StatusName = ret.Status.ToString();
        }

        return ret;
    }

    /// <summary>
    /// Busca os dados de um usuario
    /// </summary>
    [HttpGet("Get/{userName}"), Authorize]
    public UserDataReturn GetUserData(string userName)
    {
        UserDataReturn ret = new ();
        User? user = DataBase.User.FindUserByName(userName);
        if(user == null)
        {
            ret.Status = UserDataReturnStatus.UserNotFound;
            ret.StatusName = ret.Status.ToString();
            ret.Found = false;
            return ret;
        }
        ret.Status = UserDataReturnStatus.FoundUser;
        ret.Found = true;
        ret.UserName = user.UserName;
        ret.BirthDay = user.BirthDay;
        if(CurrentUser()?.UserName == user.UserName)
        {
            ret.Status = UserDataReturnStatus.CurrentUser;
            ret.Email = user.Email;
        }
        ret.StatusName = ret.Status.ToString();
        return ret;
    }

    /// <summary>
    /// Altera a senha do usuario logado
    /// </summary>
    [HttpPost("ChangePassword"), Authorize]
    public ChangePasswordReturn PostChangePassword([FromBody]ChangePasswordRequest req)
    {
        ChangePasswordReturn ret = new();
        User? user = CurrentUser();
        if (user == null)
        {
            ret.Status = ChangePasswordReturnStatus.InvalidUser;
            ret.StatusName = ret.Status.ToString();
            ret.ChangedPassword = false;
            return ret;
        }

        if (!user.ValidatePassword(req.OldPassword))
        {
            ret.Status = ChangePasswordReturnStatus.IncorrectPassword;
            ret.StatusName = ret.Status.ToString();
            ret.ChangedPassword = false;
            return ret;
        }

        if (!DataBase.User.MatchPasswordRequirements(req.NewPassword))
        {
            ret.Status = ChangePasswordReturnStatus.PasswordDoesntMeetRequirements;
            ret.StatusName = ret.Status.ToString();
            ret.ChangedPassword = false;
            return ret;
        }

        user.Password = DataBase.User.HashPassword(DataBase.User.GenerateSalt(), req.NewPassword);
        user.Update();
        return ret;
    }
}
