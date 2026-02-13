namespace API_Shopping.Models
{
    public class ShoppingCart
    {
        public long Id { get; set; }
        public string Status { get; set; }
        public long UserId { get; set; }
        public DateTime? CreateAt { get; set; }
        public User User { get; set; }
    }
}
