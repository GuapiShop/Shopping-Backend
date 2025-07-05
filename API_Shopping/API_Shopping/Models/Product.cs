namespace API_Shopping.Models
{
    public class Product
    {
        public long ?Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string CodeCABYS { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
