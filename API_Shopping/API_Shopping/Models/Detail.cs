namespace API_Shopping.Models
{
    public class Detail
    {
        public long ?Id { get; set; }
        public int Quantity{ get; set; }
        public int Price{ get; set; }
        public int Total { get; set;  }
        public DateTime? UpdateAt { get; set; }

        public long OrderId { get; set; }
        public Order Order { get; set; }

        public long ProductId { get; set; }
        public Product Product { get; set; }
    }
}
