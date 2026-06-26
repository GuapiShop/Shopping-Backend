using System.ComponentModel.DataAnnotations;

namespace API_Shopping.DTOs.ShoppingCart
{
    public class ItemShoppingCartCreateDTO
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Unit price cannot be negative.")]
        public int UnitPrice { get; set; }

        [Required]
        [Range(1, long.MaxValue, ErrorMessage = "ProductId must be a positive number.")]
        public long ProductId { get; set; }
    }
}