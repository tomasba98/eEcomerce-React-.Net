using eEcomerce.BackEnd.Entities.User;

namespace eEcomerce.BackEnd.Services.Users
{
    /// <summary>
    /// Defines the contract for a service that manages user entities.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Creates a new user entity.
        /// </summary>
        /// <param name="userEntity">The user entity to be created.</param>
        /// <returns>True if the user was successfully created; otherwise, false.</returns>
        bool CreateUser(User userEntity);

        /// <summary>
        /// Retrieves a user by their username.
        /// </summary>
        /// <param name="userName">The username of the user to retrieve.</param>
        /// <returns>The user entity if found; otherwise, null.</returns>
        User? GetUserByName(string userName);

        /// <summary>
        /// Retrieves a user by their unique identifier.
        /// </summary>
        /// <param name="userId">The identifier of the user to retrieve.</param>
        /// <returns>The user entity if found; otherwise, null.</returns>
        User? GetUserById(Guid userId);

        bool CheckIfUsernameExists(string userName);
    }
}
