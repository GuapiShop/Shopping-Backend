namespace API_Shopping.DTOs.ShoppingCart
{
    public class ItemShoppingCartCreateDTO
    {
        public int Quantity { get; set; }
        public int UnitPrice { get; set; }
        public long productId { get; set; }
        public long shoppingCartId { get; set; }
    }
}
