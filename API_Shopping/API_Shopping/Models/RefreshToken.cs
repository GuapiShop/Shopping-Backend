namespace API_Shopping.Models
{
    public class RefreshToken
    {
        public long Id { get; set; }
        public required string Token { get; set; }
        public long UserId { get; set; }
        public User? User { get; set; }
        public DateTime ExpiresAt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsRevoked { get; set; } = false;

        public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
        public bool IsActive => !IsRevoked && !IsExpired;
    }
}