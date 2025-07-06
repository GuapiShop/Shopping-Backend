using API_Shopping.Models;

namespace API_Shopping.Interfaces
{
    public interface IUserService
    {
        public Task<IEnumerable<User>> GetUsers();
        public Task<User> GetUserById(long id);
        public Task<User> AddUser(UserCreateDTO user);
        public Task<bool> UpdateUser(long id, User user);
        public Task<bool> DisableUser(long id, User user);
        public Task<bool> EnableUser(long id, User user);
        public Task<bool> UserExists(long id);
    }
}
