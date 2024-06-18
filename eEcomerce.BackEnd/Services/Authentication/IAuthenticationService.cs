using eEcomerce.BackEnd.Models.Authentication;

namespace eEcomerce.BackEnd.Services.Authentication;


/// <summary>
/// Defines the contract for an authentication service that generates JWT tokens.
/// </summary>
public interface IAuthenticationService
{
    /// <summary>
    /// Generates a JWT token for the specified user.
    /// </summary>
    /// <param name="user">The user for whom the JWT token will be generated.</param>
    /// <returns>An AuthenticationResponse containing the generated JWT token.</returns>
    AuthenticationResponse GenerateJwt(Entities.User.User user);
}