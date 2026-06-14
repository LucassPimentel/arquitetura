namespace Order.Domain.Errors
{
    public class CouponErrors
    {
        public static readonly Error NonExistent = new("COUPON_NON_EXISTENT", "O cupom não foi encontrado.");
        public static readonly Error CodeRequired = new("COUPON_CODE_REQUIRED", "O código do cupom é obrigatório.");
        public static readonly Error CodeTooLong = new("COUPON_CODE_TOO_LONG", "O código do cupom deve ter no máximo 20 caracteres.");
        public static readonly Error NameRequired = new("COUPON_NAME_REQUIRED", "O nome do cupom é obrigatório.");
        public static readonly Error NameTooLong = new("COUPON_NAME_TOO_LONG", "O nome do cupom deve ter no máximo 100 caracteres.");
        public static readonly Error InvalidDiscount = new("COUPON_INVALID_DISCOUNT", "O desconto fixo não pode ser negativo.");
        public static readonly Error InvalidPercentualDiscount = new("COUPON_INVALID_PERCENTUAL", "O desconto percentual deve estar entre 0 e 100.");
        public static readonly Error NoDiscountDefined = new("COUPON_NO_DISCOUNT", "O cupom deve ter pelo menos um tipo de desconto (fixo ou percentual).");
        public static readonly Error InvalidMaxDiscount = new("COUPON_INVALID_MAX_DISCOUNT", "O desconto máximo deve ser maior que zero.");
        public static readonly Error InvalidExpirationDate = new("COUPON_INVALID_EXPIRATION", "A data de expiração deve ser futura.");
        public static readonly Error Inactive = new("COUPON_INACTIVE", "O cupom está inativo.");
        public static readonly Error Expired = new("COUPON_EXPIRED", "O cupom está expirado.");
        public static readonly Error AlreadyInactive = new("COUPON_ALREADY_INACTIVE", "O cupom já está inativo.");
        public static readonly Error AlreadyActive = new("COUPON_ALREADY_ACTIVE", "O cupom já está ativo.");
        public static readonly Error AlreadyExists = new("COUPON_ALREADY_EXISTS", "Já existe um cupom com este código.");
    }
}
