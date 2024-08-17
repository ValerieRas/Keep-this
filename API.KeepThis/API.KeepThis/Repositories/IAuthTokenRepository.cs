using API.KeepThis.Model;

namespace API.KeepThis.Repositories
{
    public interface IAuthTokenRepository
    {
        Task AddTokenAsync(AuthentificationToken token);
        Task<AuthentificationToken> GetTokenByUserIdAsync(string userId);
        Task RemoveTokenAsync(AuthentificationToken token);
    }
}
