using BCrypt.Net;

namespace API.KeepThis.Helpers
{
    public class PasswordSecurity : IPasswordSecurity
    {
        public string HashPassword(string password, string salt)
        {
            // Combine the password with the salt and hash it
            return BCrypt.Net.BCrypt.HashPassword(password + salt);
        }

        public bool VerifyPassword(string hashedPassword, string password, string salt)
        {
            // Verify the password by combining it with the salt and comparing with the hash
            return BCrypt.Net.BCrypt.Verify(password + salt, hashedPassword);
        }

        public string GenerateSalt()
        {
            // Generate a salt using BCrypt's built-in method
            return BCrypt.Net.BCrypt.GenerateSalt();
        }
    }
}
