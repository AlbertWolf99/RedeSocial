using System.Security.Claims;
using Microsoft.Owin.Security.OAuth;

namespace RedeSocial;

public class TokenProvider : OAuthAuthorizationServerProvider
{
    
    public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
    {
        context.Validated();
    }

    
    public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
    {
        if (DataBase.User.Authenticate(context.UserName, context.Password))
        {
            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim("sub", context.UserName));
            identity.AddClaim(new Claim("role", "user"));
            context.Validated(identity);
        }
        else
        {
            context.SetError("Access Denied", "Username and/or password is invalid");
            return;
        }
    }

}