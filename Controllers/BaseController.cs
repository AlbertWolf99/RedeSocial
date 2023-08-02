using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace RedeSocial.Controllers;

public abstract class BaseController : ControllerBase
{
    protected DataBase.User? CurrentUser()
    {
        return DataBase.User.FindUserByName(User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).Select( c => c).First().Value);
    }

    protected bool Logged()
    {
        return CurrentUser() != null;
    }
}
