using System.ComponentModel.DataAnnotations;

namespace Order.Application.DTOs.Order
{
    public class UpdateOrderDto
    {
        [Required(ErrorMessage = "O ID do pedido é obrigatório.")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Os itens do pedido são obrigatórios.")]
        public List<CreateOrderItemDto> Items { get; set; } = new();

        public Guid? CouponId { get; set; }
    }
}
