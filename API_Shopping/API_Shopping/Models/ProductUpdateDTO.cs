namespace API_Shopping.Models
{
    public class ProductUpdateDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        public string CodeCabys { get; set; }
        public string DescriptionCabys { get; set; }
        public decimal TaxCabys { get; set; }
    }
}

