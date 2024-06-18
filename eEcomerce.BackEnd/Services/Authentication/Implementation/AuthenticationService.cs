using eEcomerce.BackEnd.Models.Authentication;
using eEcomerce.BackEnd.Utils;

namespace eEcomerce.BackEnd.Services.Authentication.Implementation;

public class AuthenticationService : IAuthenticationService
{

    public AuthenticationService()
    {

    }

    public AuthenticationResponse GenerateJwt(Entities.User.User user)
    {

        string token = Encrypt.GenerateToken(user);

        return new AuthenticationResponse(user.UserName, token);
    }

}
