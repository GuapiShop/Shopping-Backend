using API_Shopping.Context;
using API_Shopping.Interfaces;
using API_Shopping.Models;
using Microsoft.EntityFrameworkCore;

namespace API_Shopping.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context) { 
            _context = context;
        }

        public async Task<User> AddUser(UserCreateDTO user)
        {
            var userTemp = new User
            {
                Username = user.Username,
                Password = BCrypt.Net.BCrypt.HashPassword(user.Password),
                Email = user.Email,
                IsActive = true,
                CreateAt = DateTime.Now,
            };
            _context.Users.Add(userTemp);
            await _context.SaveChangesAsync();
            return userTemp;
        }

        public async Task<bool> DisableUser(long id)
        {
            UserDTO userTemp = await GetUserById(id);
            try
            {
                userTemp.IsActive = false;
                userTemp.UpdateAt = DateTime.Now;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
        }

        public async Task<bool> EnableUser(long id)
        {
            UserDTO userTemp = await GetUserById(id);
            try
            {
                userTemp.IsActive = true;
                userTemp.UpdateAt = DateTime.Now;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
        }

        public async Task<bool> UserExists(long id)
        {
            return await _context.Users.AnyAsync(e => e.Id == id);
        }

        public async Task<UserDTO> GetUserById(long id)
        {
            return await _context.Users
                .Where(u => u.Id == id)
                .Select(u => new UserDTO
                {
                    Id = u.Id,
                    Email = u.Email,
                    CreateAt = u.CreateAt,
                    UpdateAt = u.UpdateAt,
                    IsActive = u.IsActive,
                    Username = u.Username,
                }).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<UserDTO>> GetUsers()
        {
            return await _context.Users.Select( 
                u => new UserDTO {
                    Id = u.Id,
                    Email = u.Email,
                    CreateAt = u.CreateAt,
                    IsActive = u.IsActive,
                    UpdateAt = u.UpdateAt,
                    Username = u.Username
            }).ToListAsync();
        }

        public async Task<bool> UpdateUser(long id, UpdateUserDTO user)
        {
            UserDTO userTemp = await GetUserById(id);

            if (userTemp == null) { return false; }

            try
            {
                userTemp.Username = user.Username;
                userTemp.Email = user.Email;
                userTemp.UpdateAt = DateTime.Now;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException) 
            { 
                return false;
            }
        }

    }
}
