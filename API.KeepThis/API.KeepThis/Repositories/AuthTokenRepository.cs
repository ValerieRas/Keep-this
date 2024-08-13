using API.KeepThis.Data;
using API.KeepThis.Model;
using Microsoft.EntityFrameworkCore;

namespace API.KeepThis.Repositories
{
    public class AuthTokenRepository: IAuthTokenRepository
    {
        private readonly KeepThisDbContext _context;

        public AuthTokenRepository(KeepThisDbContext context)
        {
            _context = context;
        }

        public async Task AddTokenAsync(AuthentificationToken token)
        {
            _context.AuthentificationTokens.Add(token);
            await _context.SaveChangesAsync();
        }

        public async Task<AuthentificationToken> GetTokenByUserIdAsync(string userId)
        {
            return await _context.AuthentificationTokens.SingleOrDefaultAsync(t => t.IdUser== userId);
        }

        public async Task RemoveTokenAsync(string userId)
        {
            var token = await GetTokenByUserIdAsync(userId);
            if (token != null)
            {
                _context.AuthentificationTokens.Remove(token);
                await _context.SaveChangesAsync();
            }
        }
    }
}
