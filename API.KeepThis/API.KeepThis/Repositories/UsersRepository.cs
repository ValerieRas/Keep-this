using API.KeepThis.Data;
using API.KeepThis.Model;
using Microsoft.EntityFrameworkCore;

namespace API.KeepThis.Repositories
{
    public class UsersRepository:IUsersRepository
    {
        private readonly KeepThisDbContext _context;

        public UsersRepository(KeepThisDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentException("Email cannot be null or empty.", nameof(email));
            }

            // Search for the user by either CertifiedEmailUser or TemporaryEmail
            var user = await _context.Users
                .SingleOrDefaultAsync(u => u.CertifiedEmailUser == email || u.TempEmailUser == email);

            if (user == null)
            {
                return null;
            }

            return user;
        }
    }
}
