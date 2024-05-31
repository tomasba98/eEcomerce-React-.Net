namespace eEcomerce.BackEnd.Models.Authentication;

/// <summary>
/// Represents a request for access.
/// </summary>
public class AccessRequest
{
    /// <summary>
    /// Gets or sets the username.
    /// </summary>
    public required string UserName { get; set; }

    /// <summary>
    /// Gets or sets the password.
    /// </summary>
    public required string Password { get; set; }
}

