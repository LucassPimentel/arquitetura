using System.ComponentModel.DataAnnotations;

namespace Order.Application.DTOs.Product
{
    public class UpdateProductDto
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório.")]
        [MaxLength(100, ErrorMessage = "O nome pode ter no máximo 100 caracteres.")]
        public string Name { get; set; } = string.Empty;

        [Range(0.01, double.MaxValue, ErrorMessage = "O preço deve ser maior que zero.")]
        public decimal Price { get; set; }

        public bool Active { get; set; }
    }
}
