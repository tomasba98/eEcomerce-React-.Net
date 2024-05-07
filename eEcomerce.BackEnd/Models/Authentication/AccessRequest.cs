namespace eEcomerce.BackEnd.Models.UserModels
{
    /// <summary>
    /// Represents a request for access.
    /// </summary>
    public class AccessRequest
    {
        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        public string Password { get; set; }

        public string Email { get; set; }
    }
}
