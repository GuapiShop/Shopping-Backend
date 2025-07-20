using API_Shopping.Context;
using API_Shopping.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace API_Shopping.Services
{
    public class JwtService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public JwtService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public string EncryptationSHA256(string text) {
            SHA256 sHA256 = SHA256.Create();
            byte[] bytes = sHA256.ComputeHash(Encoding.UTF8.GetBytes(text));

            StringBuilder sb = new StringBuilder();
            foreach (byte b in bytes) {
                sb.Append(b);
            }
            return sb.ToString();
        }

        public string JWTGenerator(User user) {
            var userClaims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
            };
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtService:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            var jwtConfig = new JwtSecurityToken(
                claims: userClaims, 
                expires: DateTime.UtcNow.AddMinutes(Int32.Parse(_configuration["JwtService:Duration"]!)),
                signingCredentials: credentials
                );
            return new JwtSecurityTokenHandler().WriteToken(jwtConfig);
        }
        public async Task<User> FindUserWithLogin(LoginDTO login) {
            var findUser = await _context.Users
                .Where(u =>
                u.Email == login.Email
                ).FirstOrDefaultAsync();

            if (findUser != null && BCrypt.Net.BCrypt.Verify(login.Password, findUser.Password)) 
            {
                return findUser;
            }
            return null;
        }
    }
}
