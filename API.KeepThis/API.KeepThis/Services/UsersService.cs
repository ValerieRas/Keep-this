using API.KeepThis.Helpers;
using API.KeepThis.Model;
using API.KeepThis.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;

namespace API.KeepThis.Services
{
    public class UsersService : IUsersService
    {
        private readonly IUsersRepository _UsersRepository;
        private readonly IPasswordHasher _passwordHasher;
        public UsersService(IUsersRepository UsersRepository, IPasswordHasher passwordHasher)
        {
            _UsersRepository = UsersRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<User> AuthenticateUserAsync(LoginRequest request)
        {
            var user = await _UsersRepository.GetByEmailAsync(request.Email);

            if (user == null || !_passwordHasher.VerifyPassword(user.PasswordUser, request.Password))
            {
                return null;
            }

            return new User { CertifiedEmailUser = user.CertifiedEmailUser };
        }
    }
}
