using eEcomerce.BackEnd.Entities.User;
using eEcomerce.BackEnd.Models.UserModels;
using eEcomerce.BackEnd.Utils;


namespace eEcomerce.BackEnd.Services.Authentication.Implementation.AuthenticationService;

using eEcomerce.BackEnd.Services.Authentication.IAuthenticationService;
public class AuthenticationService : IAuthenticationService
{

    public AuthenticationService()
    {

    }

    public AuthenticationResponse GenerateJwt(User user)
    {

        string token = Encrypt.GenerateToken(user);

        return new AuthenticationResponse(user.UserName, token);
    }

}
