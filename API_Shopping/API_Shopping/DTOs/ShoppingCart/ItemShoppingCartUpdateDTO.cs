namespace API_Shopping.DTOs.ShoppingCart
{
    public class ItemShoppingCartUpdateDTO
    {
        public long Id { get; set; }
        public int Quantity { get; set; }
        public long ProductId { get; set; }
        public long ShoppingCartId { get; set; }
    }
}
