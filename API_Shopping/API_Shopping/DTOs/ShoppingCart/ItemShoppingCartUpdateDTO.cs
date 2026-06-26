using System.ComponentModel.DataAnnotations;

namespace API_Shopping.DTOs.ShoppingCart
{
    public class ItemShoppingCartUpdateDTO
    {
        [Required]
        [Range(1, long.MaxValue, ErrorMessage = "Id must be a positive number.")]
        public long Id { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Quantity cannot be negative.")]
        public int Quantity { get; set; }

        [Required]
        [Range(1, long.MaxValue, ErrorMessage = "ProductId must be a positive number.")]
        public long ProductId { get; set; }

        [Required]
        [Range(1, long.MaxValue, ErrorMessage = "ShoppingCartId must be a positive number.")]
        public long ShoppingCartId { get; set; }
    }
}