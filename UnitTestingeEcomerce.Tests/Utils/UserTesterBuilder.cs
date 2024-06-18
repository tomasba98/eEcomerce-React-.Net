using eEcomerce.BackEnd.Entities.User;

using Microsoft.AspNetCore.Http;

using Moq;

using System.Security.Claims;

namespace UnitTestingeEcomerce.Tests.Utils
{
    /// <summary>
    /// Builder class for creating User instances for testing purposes.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="UserTesterBuilder"/> class.
    /// </remarks>
    /// <param name="mockHttpContextAccessor">The mocked HTTP context accessor.</param>
    public class UserTesterBuilder(Mock<IHttpContextAccessor> mockHttpContextAccessor)
    {
        private readonly User _user = new();
        private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor = mockHttpContextAccessor;

        /// <summary>
        /// Sets the Id of the User.
        /// </summary>
        /// <param name="id">The user Id.</param>
        /// <returns>The current instance of <see cref="UserTesterBuilder"/>.</returns>
        public UserTesterBuilder WithId(Guid id)
        {
            _user.Id = id;
            return this;
        }

        /// <summary>
        /// Sets the UserName of the User.
        /// </summary>
        /// <param name="userName">The user name.</param>
        /// <returns>The current instance of <see cref="UserTesterBuilder"/>.</returns>
        public UserTesterBuilder WithUserName(string userName)
        {
            _user.UserName = userName;
            return this;
        }

        /// <summary>
        /// Sets the Password of the User.
        /// </summary>
        /// <param name="password">The user password.</param>
        /// <returns>The current instance of <see cref="UserTesterBuilder"/>.</returns>
        public UserTesterBuilder WithPassword(string password)
        {
            _user.Password = password;
            return this;
        }

        /// <summary>
        /// Sets the Email of the User.
        /// </summary>
        /// <param name="email">The user email.</param>
        /// <returns>The current instance of <see cref="UserTesterBuilder"/>.</returns>
        public UserTesterBuilder WithEmail(string email)
        {
            _user.Email = email;
            return this;
        }

        /// <summary>
        /// Builds and returns the constructed <see cref="User"/> instance.
        /// </summary>
        /// <returns>The constructed <see cref="User"/> instance.</returns>
        public User Build()
        {
            return _user;
        }

        /// <summary>
        /// Sets up the user in the HTTP context.
        /// </summary>
        public void SetupUserInHttpContext()
        {
            List<Claim> claims = new() { new Claim("UserId", _user.Id.ToString()) };
            ClaimsIdentity identity = new(claims, "TestAuthType");
            ClaimsPrincipal claimsPrincipal = new(identity);
            DefaultHttpContext context = new() { User = claimsPrincipal };

            _mockHttpContextAccessor.Setup(x => x.HttpContext).Returns(context);
        }
    }
}
