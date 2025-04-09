using eEcomerce.BackEnd.Services.User;

using Microsoft.AspNetCore.Mvc;

using System.Security.Claims;

namespace eEcomerce.BackEnd.Controllers;

public abstract class BaseController : ControllerBase
{
    protected readonly IHttpContextAccessor _httpContextAccessor;
    protected readonly IUserService _userService;

    protected BaseController(IHttpContextAccessor httpContextAccessor, IUserService userService)
    {
        _httpContextAccessor = httpContextAccessor;
        _userService = userService;
    }

    protected Guid? GetUserIdFromToken()
    {
        Claim? userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst("UserId");
        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userId))
        {
            return null;
        }
        return userId;
    }

    protected bool ValidateUserId(Guid? userId)
    {
        if (userId == null || _userService.GetUserById(userId.Value) == null)
        {
            return false;
        }
        return true;
    }
}