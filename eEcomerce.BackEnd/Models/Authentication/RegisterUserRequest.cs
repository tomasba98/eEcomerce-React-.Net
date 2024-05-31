namespace eEcomerce.BackEnd.Models.Authentication;

public class RegisterUserRequest
{
    public required string UserName { get; set; }

    public required string Password { get; set; }

    public required string Email { get; set; }
}
