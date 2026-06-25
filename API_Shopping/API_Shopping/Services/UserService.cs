using API_Shopping.Context;
using API_Shopping.DTOs.User;
using API_Shopping.Exceptions.User;
using API_Shopping.Interfaces;
using API_Shopping.Models;
using Microsoft.EntityFrameworkCore;

namespace API_Shopping.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User> AddUser(UserCreateDTO user)
        {
            bool emailTaken = await _context.Users.AnyAsync(u => u.Email == user.Email);
            if (emailTaken)
                throw new UserAlreadyExistsException("email", user.Email);

            bool usernameTaken = await _context.Users.AnyAsync(u => u.Username == user.Username);
            if (usernameTaken)
                throw new UserAlreadyExistsException("username", user.Username);

            var userTemp = new User
            {
                Username = user.Username,
                Password = BCrypt.Net.BCrypt.HashPassword(user.Password),
                Email = user.Email,
                IsActive = true,
                CreateAt = DateTime.UtcNow,
                Role = "client",
            };

            _context.Users.Add(userTemp);
            await _context.SaveChangesAsync();
            return userTemp;
        }

        public async Task DisableUser(long id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id)
                ?? throw new UserNotFoundException(id);

            if (user.IsActive == false)
                throw new UserAlreadyDisabledException(id);

            user.IsActive = false;
            user.UpdateAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }

        public async Task EnableUser(long id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id)
                ?? throw new UserNotFoundException(id);

            if (user.IsActive == true)
                throw new UserAlreadyEnabledException(id);

            user.IsActive = true;
            user.UpdateAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UserExists(long id)
        {
            return await _context.Users.AnyAsync(e => e.Id == id);
        }

        public async Task<UserDTO> GetUserById(long id)
        {
            var user = await _context.Users
                .Where(u => u.Id == id)
                .Select(u => new UserDTO
                {
                    Id = u.Id,
                    Email = u.Email,
                    CreateAt = u.CreateAt,
                    UpdateAt = u.UpdateAt,
                    IsActive = u.IsActive,
                    Username = u.Username,
                    Role = u.Role,
                }).FirstOrDefaultAsync();

            if (user == null)
                throw new UserNotFoundException(id);

            return user;
        }

        public async Task<object> GetUsers(int page = 1, int pageSize = 10)
        {
            if (page < 1 || pageSize < 1)
                throw new InvalidPaginationException();

            var query = _context.Users.AsQueryable();

            var totalItems = await query.CountAsync();
            var totalPage = (int)Math.Ceiling(totalItems / (double)pageSize);

            var users = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(u => new UserDTO
                {
                    Id = u.Id,
                    Email = u.Email,
                    CreateAt = u.CreateAt,
                    IsActive = u.IsActive,
                    UpdateAt = u.UpdateAt,
                    Username = u.Username,
                    Role = u.Role,
                })
                .ToListAsync();

            return new { page, totalPage, data = users };
        }

        public async Task UpdateUser(long id, UserUpdateDTO userDto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id)
                ?? throw new UserNotFoundException(id);

            bool emailTaken = await _context.Users
                .AnyAsync(u => u.Email == userDto.Email && u.Id != id);
            if (emailTaken)
                throw new UserAlreadyExistsException("email", userDto.Email);

            bool usernameTaken = await _context.Users
                .AnyAsync(u => u.Username == userDto.Username && u.Id != id);
            if (usernameTaken)
                throw new UserAlreadyExistsException("username", userDto.Username);

            user.Username = userDto.Username;
            user.Email = userDto.Email;
            user.UpdateAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }
    }
}