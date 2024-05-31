using eEcomerce.BackEnd.Entities.User;
using eEcomerce.BackEnd.Models.Authentication;
using eEcomerce.BackEnd.Services.Authentication.IAuthenticationService;
using eEcomerce.BackEnd.Services.Users;
using eEcomerce.BackEnd.Utils;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eEcomerce.BackEnd.Controllers.AuthenticationController
{
    /// <summary>
    /// Controller for user authentication.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthenticationController : BaseController
    {
        private readonly IAuthenticationService _authenticationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationController"/> class.
        /// </summary>
        /// <param name="authenticationService">The authentication service.</param>
        /// <param name="userService">The user service.</param>
        public AuthenticationController(IAuthenticationService authenticationService, IUserService userService, IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor, userService)
        {
            _authenticationService = authenticationService;
        }

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="request">The registration request.</param>
        /// <returns>An action result indicating the registration status.</returns>
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

        /// <summary>
        /// Logs in a user.
        /// </summary>
        /// <param name="request">The login request.</param>
        /// <returns>An action result containing authentication response.</returns>
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
}
