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
       
        public UsersService(IUsersRepository UsersRepository)
        {
            _UsersRepository = UsersRepository;
           
        }

    }
}
