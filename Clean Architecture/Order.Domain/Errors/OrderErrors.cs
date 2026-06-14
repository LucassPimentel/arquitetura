namespace Order.Domain.Errors
{
    public class OrderErrors
    {
        public static readonly Error EmptyOrder = new("ORDER_EMPTY", "O pedido deve ter pelo menos um item.");
        public static readonly Error InvalidClientId = new("ORDER_INVALID_CLIENT_ID", "O ID do cliente deve ser maior que zero.");
        public static readonly Error NameTooLong = new("ORDER_NAME_TOO_LONG", "O nome do pedido deve possuir no máximo 100 caracteres.");
        public static readonly Error InvalidPrice = new("ORDER_INVALID_PRICE", "O preço do pedido deve ser maior que zero.");
        public static readonly Error NonExistent = new("ORDER_NON_EXISTENT", "O pedido não foi encontrado.");
        public static readonly Error AlreadyExists = new("ORDER_ALREADY_EXISTS", "O pedido já existe.");
        public static readonly Error Canceled = new("ORDER_CANCELED", "O pedido está cancelado e não pode ser modificado.");
        public static readonly Error Finished = new("ORDER_FINISHED", "O pedido está finalizado e não pode ser modificado.");
        public static readonly Error CouponAlreadyApplied = new("ORDER_COUPON_ALREADY_APPLIED", "Um cupom já foi aplicado a este pedido.");
        public static readonly Error ItemNotFound = new("ORDER_ITEM_NOT_FOUND", "O item não foi encontrado no pedido.");
    }
}

