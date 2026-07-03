namespace API_Shopping.DTOs.Detail
{
    public class DetailProductDTO
    {
        public long ProductId { get; set; }
        public string ProductName { get; set; }
        public string Category { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Total { get; set; }
    }
}