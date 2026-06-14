using System.ComponentModel.DataAnnotations;

namespace Order.Application.DTOs.Order
{
    public class CreateOrderDto
    {
        [Required(ErrorMessage = "O ID do cliente é obrigatório.")]
        public int ClientId { get; set; }

        [Required(ErrorMessage = "Os itens do pedido são obrigatórios.")]
        public List<CreateOrderItemDto> Items { get; set; } = new();

        public Guid? CouponId { get; set; }
    }

    public class CreateOrderItemDto
    {
        public Guid ProductId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "A quantidade deve ser maior que zero.")]
        public int Quantity { get; set; }
    }
}

