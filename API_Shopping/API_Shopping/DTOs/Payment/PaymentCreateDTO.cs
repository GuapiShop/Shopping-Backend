namespace API_Shopping.DTOs.Payment
{
    public class PaymentCreateDTO
    {
        public string Type { get; set; }
        public double Total { get; set; }
        public long OrderId { get; set; }
    }
}
