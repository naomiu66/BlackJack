using UserService.Data.Models;
using UserService.Data.Repositories.Abstractions;

namespace UserService.Data.Repositories.Implementations
{
    public class UsersRepository : Repository<User>, IUsersRepository
    {
        public UsersRepository(ApplicationContext context) : base(context)
        {
        }
    }
}
