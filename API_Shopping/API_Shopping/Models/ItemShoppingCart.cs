using System.Runtime.InteropServices.Marshalling;

namespace API_Shopping.Models
{
    public class ItemShoppingCart
    {
        public long Id { get; set; }
        public int Quantity { get; set; }
        public int UnitPrice { get; set; }
        public long productId { get; set; }
        public long shoppingCartId { get; set; }
        public Product Product { get; set; }
        public ShoppingCart ShoppingCart { get; set; }
    }
}
