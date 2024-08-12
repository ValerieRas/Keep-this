using API.KeepThis.Data;

namespace API.KeepThis.Repositories
{
    public class UsersRepository:IUsersRepository
    {
        private readonly KeepThisDbContext _context;

        public UsersRepository(KeepThisDbContext context)
        {
            _context = context;
        }
    }
}
