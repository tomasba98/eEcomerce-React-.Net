using eEcomerce.BackEnd.Entities.User;

using Microsoft.IdentityModel.Tokens;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace eEcomerce.BackEnd.Utils
{
    /// <summary>
    /// Utility class for generating JWT tokens.
    /// </summary>
    public static class Encrypt
    {
        /// <summary>
        /// Generates a JWT token for the specified user.
        /// </summary>
        /// <param name="user">The user for whom the token is generated.</param>
        /// <returns>The generated JWT token.</returns>
        public static string GenerateToken(User user)
        {
            ArgumentNullException.ThrowIfNull(user);

            byte[] keyBytes = Encoding.UTF8.GetBytes("!£@0#y~9I1.p0goq1£1+12345678901234567890123456789012");

            SigningCredentials signingCredentials = new(
                new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256);

            Claim[] claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Name, user.UserName),
                new Claim("UserId", user.Id.ToString())
            };

            JwtSecurityToken securityToken = new(
                issuer: "eEcomerce",
                expires: DateTime.Now.AddHours(3),
                claims: claims,
                signingCredentials: signingCredentials);

            return new JwtSecurityTokenHandler().WriteToken(securityToken);
        }

        public static string Hash(string value)
        {
            StringBuilder sb = new();
            Encoding enc = Encoding.UTF8;

            byte[] result = SHA256.HashData(enc.GetBytes(value));

            foreach (byte b in result)
            {
                sb.Append(b.ToString("x2"));
            }

            return sb.ToString();
        }

        /// <summary>
        /// Checks if a given plain text password matches a hashed password.
        /// </summary>
        /// <param name="plainTextPassword">The plain text password to check.</param>
        /// <param name="hashedPassword">The hashed password to compare against.</param>
        /// <returns>True if the passwords match, otherwise false.</returns>
        public static bool CheckHash(string plainTextPassword, string hashedPassword)
        {
            string hashedInput = Hash(plainTextPassword);
            return string.Equals(hashedInput, hashedPassword, StringComparison.OrdinalIgnoreCase);
        }

    }
}