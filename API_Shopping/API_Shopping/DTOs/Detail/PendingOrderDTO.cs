namespace API_Shopping.DTOs.Detail
{
    public class PendingOrderDTO
    {
        public long OrderId { get; set; }
        public string State { get; set; }
        public DateTime? CreatedAt { get; set; }
        public List<DetailProductDTO> Items { get; set; } = new();
        public decimal GrandTotal { get; set; }
    }
}