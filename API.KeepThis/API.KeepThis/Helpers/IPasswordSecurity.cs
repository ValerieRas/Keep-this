namespace API.KeepThis.Helpers
{
    public interface IPasswordSecurity
    {
        string HashPassword(string password, string salt);
        bool VerifyPassword(string hashedPassword, string password, string salt);

        string GenerateSalt();
    }
}
