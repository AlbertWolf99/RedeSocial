using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace RedeSocial.Controllers;

public abstract class BaseController : ControllerBase
{
    /// <summary>
    /// Busca pelo usuario logado atualmente
    /// </summary>
    /// <returns>
    /// O usuario logado atualmente ou null caso não exista
    /// </returns>
    protected DataBase.User? CurrentUser()
    {
        return DataBase.User.FindUserByName(User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).Select( c => c).First().Value);
    }

    /// <summary>
    /// Checa se o usuario esta logado
    /// </summary>
    /// <returns>
    /// True se o usuario estiver logado, False caso contrario
    /// </returns>
    protected bool Logged()
    {
        return CurrentUser() != null;
    }
}
