using System.ComponentModel.DataAnnotations;

namespace API_Shopping.DTOs.Detail
{
    public class DetailCreateDTO
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }

        [Required]
        [Range(1, long.MaxValue, ErrorMessage = "ProductId must be a positive number.")]
        public long ProductId { get; set; }
    }
}