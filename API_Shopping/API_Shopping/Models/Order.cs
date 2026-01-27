namespace API_Shopping.Models
{
    public class Order
    {
        public long? Id { get; set; }
        public string State { get; set; }
        public DateTime? CreateAt { get; set; }

        public long UserId { get; set; }
        public User User { get; set; }
    }
}
