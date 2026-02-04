namespace API_Shopping.Models
{
    public class Payment
    {
        public long Id { get; set; }
        public string Type { get; set; }
        public double Total { get; set; }
        public long OrderId { get; set; }
        public DateTime? CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }
    }
}
