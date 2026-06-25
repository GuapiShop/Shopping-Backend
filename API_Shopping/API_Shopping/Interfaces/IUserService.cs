using API_Shopping.DTOs.User;
using API_Shopping.Models;

namespace API_Shopping.Interfaces
{
    public interface IUserService
    {
        public Task<User> AddUser(UserCreateDTO user);
        public Task DisableUser(long id);
        public Task EnableUser(long id);
        public Task<bool> UserExists(long id);
        public Task<UserDTO> GetUserById(long id);
        public Task<object> GetUsers( int page, int pageSize );
        public Task UpdateUser(long id, UserUpdateDTO user);
        public Task DeleteUser(long id);
    }
}
