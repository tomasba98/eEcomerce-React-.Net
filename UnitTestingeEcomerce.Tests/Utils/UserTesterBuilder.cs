using eEcomerce.BackEnd.Entities.User;

using Microsoft.AspNetCore.Http;

using Moq;

using System.Security.Claims;

namespace UnitTestingeEcomerce.Tests.Utils;

public class UserTesterBuilder
{
    private readonly User _user;
    private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;

    public UserTesterBuilder(Mock<IHttpContextAccessor> mockHttpContextAccessor)
    {
        _user = new User();
        _mockHttpContextAccessor = mockHttpContextAccessor;
    }

    public UserTesterBuilder WithId(Guid id)
    {
        _user.Id = id;
        return this;
    }

    public UserTesterBuilder WithUserName(string userName)
    {
        _user.UserName = userName;
        return this;
    }

    public UserTesterBuilder WithPassword(string password)
    {
        _user.Password = password;
        return this;
    }

    public UserTesterBuilder WithEmail(string email)
    {
        _user.Email = email;
        return this;
    }

    public User Build()
    {
        return _user;
    }

    public void SetupUserInHttpContext()
    {
        List<Claim> claims = new() { new Claim("UserId", _user.Id.ToString()) };
        ClaimsIdentity identity = new(claims, "TestAuthType");
        ClaimsPrincipal claimsPrincipal = new(identity);
        DefaultHttpContext context = new() { User = claimsPrincipal };

        _mockHttpContextAccessor.Setup(x => x.HttpContext).Returns(context);
    }
}