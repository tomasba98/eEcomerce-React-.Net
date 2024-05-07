using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eEcomerce.BackEnd.Entities.User;

/// <summary>
/// Represents a user entity.
/// </summary>
[Table("Users")]
public class User : EntityBase
{
    /// <summary>
    /// Gets or sets the username of the user.
    /// </summary>
    [Required]
    public string UserName { get; set; } = null!;

    /// <summary>
    /// Gets or sets the password of the user.
    /// </summary>
    [Required]
    public string Password { get; set; } = null!;

    [Required]
    public string Email { get; set; } = null!;
}
