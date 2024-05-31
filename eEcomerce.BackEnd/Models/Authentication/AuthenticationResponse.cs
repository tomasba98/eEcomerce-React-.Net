namespace eEcomerce.BackEnd.Models.Authentication;

/// <summary>
/// Represents an authentication response.
/// </summary>
public class AuthenticationResponse
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AuthenticationResponse"/> class.
    /// </summary>
    /// <param name="userName">The username.</param>
    /// <param name="token">The authentication token.</param>
    public AuthenticationResponse(string userName, string token)
    {
        UserName = userName;
        Token = token;
    }

    /// <summary>
    /// Gets or sets the username.
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// Gets or sets the authentication token.
    /// </summary>
    public string Token { get; set; }
}

