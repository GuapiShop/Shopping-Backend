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
                Username = user.username,
                Password = user.password,
                Email = user.email,
                CreateAt = DateTime.Now,
            };
            _context.Users.Add(userTemp);
            await _context.SaveChangesAsync();
            return userTemp;
        }

        public async Task<bool> DisableUser(long id, User user)
        {
            User userTemp = await GetUserById(id);
            if (userTemp == null) { return false; }

            try
            {
                user.IsActive = false;
                user.UpdateAt = DateTime.Now;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
        }

        public async Task<bool> EnableUser(long id, User user)
        {
            User userTemp = await GetUserById(id);
            if (userTemp == null) { return false; }

            try
            {
                user.IsActive = true;
                user.UpdateAt = DateTime.Now;
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

        public async Task<User> GetUserById(long id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<bool> UpdateUser(long id, User user)
        {
            User userTemp = await GetUserById(id);

            if (userTemp == null) { return false; }

            try
            {
                user.Username = userTemp.Username;
                user.Password = userTemp.Password;
                user.Email = userTemp.Email;
                user.UpdateAt = DateTime.Now;
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
