using eEcomerce.BackEnd.Entities.User;
using eEcomerce.BackEnd.Models.Authentication;
using eEcomerce.BackEnd.Services.Authentication;
using eEcomerce.BackEnd.Services.User;
using eEcomerce.BackEnd.Utils;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eEcomerce.BackEnd.Controllers;


[Route("api/[controller]")]
[ApiController]
[AllowAnonymous]
public class AuthenticationController : BaseController
{
    private readonly IAuthenticationService _authenticationService;


    public AuthenticationController(IAuthenticationService authenticationService, IUserService userService, IHttpContextAccessor httpContextAccessor)
        : base(httpContextAccessor, userService)
    {
        _authenticationService = authenticationService;
    }


    [HttpPost("Register")]
    public IActionResult Register(RegisterUserRequest request)
    {
        if (_userService.CheckIfUsernameExists(request.UserName))
        {
            return BadRequest("Username already in use.");
        }

        string hashPassword = Encrypt.Hash(request.Password);
        User newUser = new()
        {
            Email = request.Email,
            UserName = request.UserName,
            Password = hashPassword
        };

        bool result = _userService.CreateUser(newUser);

        return result ? Ok() : BadRequest("Something went wrong.");
    }


    [HttpPost("Login")]
    public IActionResult Login(AccessRequest request)
    {
        User? user = _userService.GetUserByName(request.UserName);

        if (user is null)
        {
            return BadRequest("Invalid credentials.");
        }

        if (!Encrypt.CheckHash(request.Password, user.Password))
        {
            return BadRequest("Invalid credentials.");
        }

        AuthenticationResponse response = _authenticationService.GenerateJwt(user);

        return Ok(response);
    }
}

