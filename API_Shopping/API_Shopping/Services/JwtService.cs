using API_Shopping.Context;
using API_Shopping.DTOs.Auth;
using API_Shopping.Exceptions.Auth;
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

        public string JWTGenerator(User user) {

            var userClaims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role),
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
                u.Email.ToLower() == login.Email.ToLower()
                ).FirstOrDefaultAsync();

            if (findUser != null && BCrypt.Net.BCrypt.Verify(login.Password, findUser.Password)) 
            {
                return findUser;
            }
            return null;
        }

        // method to test the validation of token into the Shopping.Tests
        public ClaimsPrincipal? ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtService:Key"]!));

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                IssuerSigningKey = securityKey
            };

            try
            {
                var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);

                // Extra defense-in-depth: ensure the algorithm wasn't downgraded (e.g. "none" attack)
                if (validatedToken is not JwtSecurityToken jwtToken ||
                    !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256Signature, StringComparison.InvariantCultureIgnoreCase))
                {
                    return null;
                }

                return principal;
            }
            catch
            {
                // Covers SecurityTokenExpiredException, SecurityTokenInvalidSignatureException, etc.
                return null;
            }
        }

        public async Task<RefreshToken> GenerateRefreshToken(long userId)
        {
            var refreshToken = new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                UserId = userId,
                ExpiresAt = DateTime.UtcNow.AddDays(Int32.Parse(_configuration["JwtService:RefreshTokenDurationDays"] ?? "7"))
            };

            _context.RefreshTokens.Add(refreshToken);
            await _context.SaveChangesAsync();

            return refreshToken;
        }

        public async Task<TokenResponseDTO> RefreshAccessToken(string refreshTokenValue)
        {
            var storedToken = await _context.RefreshTokens
                .Include(rt => rt.User)
                .FirstOrDefaultAsync(rt => rt.Token == refreshTokenValue);

            if (storedToken == null || !storedToken.IsActive || storedToken.User == null)
            {
                throw new InvalidRefreshTokenException();
            }

            storedToken.IsRevoked = true;

            var newAccessToken = JWTGenerator(storedToken.User);
            var newRefreshToken = await GenerateRefreshToken(storedToken.User.Id);

            await _context.SaveChangesAsync();

            return new TokenResponseDTO
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken.Token
            };
        }
    }
}
