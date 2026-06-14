namespace Order.Domain.Errors
{
    public class ProductErrors
    {
        public static readonly Error NameRequired = new("Product.NameRequired", "O nome do produto é obrigatório.");
        public static readonly Error NameTooLong = new("Product.NameTooLong", "O nome do produto deve possuir no máximo 100 caracteres.");
        public static readonly Error InvalidPrice = new("Product.InvalidPrice", "O preço deve ser maior que zero.");
        public static readonly Error NonExistent = new("Product.Non-existent", "O Produto não foi encontrado.");
        public static readonly Error AlreadyExists = new("Product.AlreadyExists", "O produto já existe.");
    }
}

