namespace API_Shopping.Models
{
    public class DetailCreateDTO
    {
        public int Quantity { get; set; }
        public int Price { get; set; }
        public int Total { get; set; }
        public long ProductId { get; set; }
    }
}