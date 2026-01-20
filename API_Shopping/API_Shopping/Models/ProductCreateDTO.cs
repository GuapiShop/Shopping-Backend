using System.ComponentModel.DataAnnotations;

namespace API_Shopping.Models
{
    public class ProductCreateDTO
    {
        [Required(ErrorMessage = "The name field is required.")]
        [StringLength(100, ErrorMessage = "The name must not exceed 100 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "The description field is required.")]
        [StringLength(500, ErrorMessage = "The description must not exceed 500 characters.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "The category field is required.")]
        [StringLength(100, ErrorMessage = "The category must not exceed 100 characters.")]
        [RegularExpression("^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]+$", ErrorMessage ="The field must contains only leters.")]
        public string Category { get; set; }

        [Required(ErrorMessage = "The CABYS field is required")]
        public string CodeCabys { get; set; }

        [Required(ErrorMessage = "The description of cabys field is required.")]
        [StringLength(500, ErrorMessage = "The description of cabys must not exceed 500 characters.")]
        public string DescriptionCabys { get; set; }

        [Range(0.01, 99999, ErrorMessage = "The price field must be between 0.01 and 99999")]
        public decimal Price { get; set; }

        [Range(0.01, 99999, ErrorMessage = "The tax of cabys field must be between 0.01 and 99999")]
        public decimal TaxCabys { get; set; }
    }
}
