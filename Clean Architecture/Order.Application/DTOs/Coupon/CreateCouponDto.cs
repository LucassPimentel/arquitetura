using System.ComponentModel.DataAnnotations;

namespace Order.Application.DTOs.Coupon
{
    public class CreateCouponDto
    {
        [Required(ErrorMessage = "O código é obrigatório.")]
        [MaxLength(20, ErrorMessage = "O código pode ter no máximo 20 caracteres.")]
        public string Code { get; set; } = string.Empty;

        [Required(ErrorMessage = "O nome é obrigatório.")]
        [MaxLength(100, ErrorMessage = "O nome pode ter no máximo 100 caracteres.")]
        public string Name { get; set; } = string.Empty;

        [Range(0, double.MaxValue, ErrorMessage = "O desconto fixo não pode ser negativo.")]
        public decimal Discount { get; set; }

        [Range(0, 100, ErrorMessage = "O desconto percentual deve estar entre 0 e 100.")]
        public decimal PercentualDiscount { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "O desconto máximo deve ser maior que zero.")]
        public decimal MaxDiscount { get; set; }

        public DateTime ExpirationDate { get; set; } = DateTime.UtcNow.AddMonths(1);
    }
}
