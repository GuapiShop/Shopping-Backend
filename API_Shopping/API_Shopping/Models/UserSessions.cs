namespace API_Shopping.Models
{
    public class UserSessions
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime ExpiredDate { get; set; }
        public DateTime RevokedAt { get; set; }
        public virtual User User { get; set; }
    }
}
