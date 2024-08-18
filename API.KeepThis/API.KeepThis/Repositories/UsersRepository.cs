using API.KeepThis.Data;
using API.KeepThis.Model;
using Microsoft.EntityFrameworkCore;

namespace API.KeepThis.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly KeepThisDbContext _context;

        public UsersRepository(KeepThisDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            // Search for the user by either CertifiedEmailUser or TemporaryEmail
            var user = await _context.Users
                .SingleOrDefaultAsync(u => u.CertifiedEmailUser == email || u.TempEmailUser == email);

            if (user == null)
            {
                return null;
            }

            return user;
        }

        public async Task<User?> GetByIdAsync(string userId)
        {
            return await _context.Users.FindAsync(userId);
        }

        public async Task UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task CreateUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }
    }
}