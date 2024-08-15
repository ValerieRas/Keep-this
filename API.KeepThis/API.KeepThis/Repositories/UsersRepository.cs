using API.KeepThis.Data;
using API.KeepThis.Model;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace API.KeepThis.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly KeepThisDbContext _context;

        public UsersRepository(KeepThisDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.CertifiedEmailUser == email || u.TempEmailUser == email);
        }

        public async Task AddUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }
    }
}
