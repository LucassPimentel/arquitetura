using Order.Domain.Entities.Base;
using Order.Domain.Errors;

namespace Order.Domain.Entities;

public sealed class Product : Entity
{
    public string Name { get; private set; } = null!;
    public decimal Price { get; private set; }
    public bool Active { get; private set; }

    private Product() { }

    private Product(Guid id, string name, decimal price)
    {
        Id = id;
        Name = name;
        Price = price;
        Active = true;
    }

    public static Result<Product> Create(string name, decimal price)
    {
        var error = Validate(name, price);
        if (error is not null)
            return Result<Product>.Failure(error);

        return Result<Product>.Success(
            new Product(Guid.NewGuid(), name.Trim(), price)
        );
    }

    public Result Update(string name, decimal price, bool active)
    {
        var error = Validate(name, price);
        if (error is not null)
            return Result.Failure(error);

        Name = name.Trim();
        Price = price;
        Active = active;

        return Result.Success();
    }

    private static Error? Validate(string name, decimal price)
    {
        if (string.IsNullOrWhiteSpace(name))
            return ProductErrors.NameRequired;

        if (name.Length > 100)
            return ProductErrors.NameTooLong;

        if (price <= 0)
            return ProductErrors.InvalidPrice;

        return null;
    }
}