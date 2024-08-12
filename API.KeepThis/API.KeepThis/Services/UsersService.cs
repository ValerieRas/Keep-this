using API.KeepThis.Helpers;
using API.KeepThis.Repositories;
using Microsoft.AspNetCore.Identity;

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
    }
}
