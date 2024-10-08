using Newtonsoft.Json;
using Shared.DDD;

namespace Basket.Basket.Models;

public sealed class ShoppingCartItem : Entity<Guid>
{
    public Guid ShoppingCartId { get; private set; }
    public Guid ProductId { get; private set; }
    public int Quantity { get; internal set; }
    public string Color { get; private set; } = default!;

    // from catalog module
    public decimal Price { get; private set; }
    public string ProductName { get; private set; } = default!;

    internal void UpdatePrice(decimal newPrice)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(newPrice);
        Price = newPrice;
    }

    internal ShoppingCartItem(Guid shoppingCartId, Guid productId, int quantity, string color, decimal price, string productName)
    {
        ShoppingCartId = shoppingCartId;
        ProductId = productId;
        Quantity = quantity;
        Color = color;
        Price = price;
        ProductName = productName;
    }

    [JsonConstructor]
    internal ShoppingCartItem(Guid id, Guid shoppingCartId, Guid productId, int quantity, string color, decimal price, string productName)
    {
        Id = id;
        ShoppingCartId = shoppingCartId;
        ProductId = productId;
        Quantity = quantity;
        Color = color;
        Price = price;
        ProductName = productName;
    }

}