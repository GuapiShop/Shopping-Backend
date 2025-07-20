using API_Shopping.Models;

namespace API_Shopping.Interfaces
{
    public interface IUserService
    {
        public Task<IEnumerable<UserDTO>> GetUsers();
        public Task<UserDTO> GetUserById(long id);
        public Task<User> AddUser(UserCreateDTO user);
        public Task<bool> UpdateUser(long id, UserUpdateDTO user);
        public Task<bool> DisableUser(long id);
        public Task<bool> EnableUser(long id);
        public Task<bool> UserExists(long id);
    }
}
