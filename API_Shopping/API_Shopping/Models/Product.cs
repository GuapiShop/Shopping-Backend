namespace API_Shopping.Models
{
    public class Product
    {
        public long ?Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string CodeCabys { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }
    }
}
