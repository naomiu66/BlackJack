using UserService.Data.Models;
using UserService.Data.Repositories.Abstractions;
using UserService.Services.Abstractions;

namespace UserService.Services.Implementations
{
    public class UsersService : Service<User>, IUsersService
    {
        private readonly IUsersRepository _repository;
        public UsersService(IUsersRepository repository) : base(repository)
        {
            _repository = repository;
        }
    }
}
