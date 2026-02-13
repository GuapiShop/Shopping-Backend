namespace API_Shopping.Models
{
    public class ShoppingCart
    {
        public long Id { get; set; }
        public string Status { get; set; }
        public long userId { get; set; }
        public DateTime? CreateAt { get; set; }
        public User User { get; set; }
    }
}
