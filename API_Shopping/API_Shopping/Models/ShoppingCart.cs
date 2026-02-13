using API_Shopping.Enums;

namespace API_Shopping.Models
{
    public class ShoppingCart
    {
        public long Id { get; set; }
        public ShoppingCartStatus Status { get; set; }
        public long UserId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public User User { get; set; }
        public List<ItemShoppingCart> ItemShoppingCarts { get; set; } = new();
    }
}
