using System;
using Basket.Basket.Exceptions;
using Shared.DDD;

namespace Basket.Basket.Models;

public sealed class ShoppingCart : Aggregate<Guid>
{
    public string UserName { get; set; } = default!;
    public readonly List<ShoppingCartItem> _items = [];
    public IReadOnlyList<ShoppingCartItem> Items => _items.AsReadOnly();
    public decimal TotalPrice => Items.Sum(x => x.Price * x.Quantity);

    public static ShoppingCart Create(Guid id, string UserName)
    {
        ArgumentException.ThrowIfNullOrEmpty(UserName);

        var ShoppingCart = new ShoppingCart
        {
            Id = id,
            UserName = UserName,
        };

        return ShoppingCart;
    }

    public void AddItem(Guid productId, int quantity, string color, decimal price, string productName)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(quantity);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(price);

        var existingItems = Items.FirstOrDefault(x => x.ProductId == productId);

        if (existingItems != null)
        {
            existingItems.Quantity += quantity;
        }
        else
        {
            var newItem = new ShoppingCartItem(Id, productId, quantity, color, price, productName);
            _items.Add(newItem);
        }
    }

    public void RemoveItem(Guid productId)
    {
        var existingItems = Items.FirstOrDefault(x => x.ProductId == productId)
                            ?? throw new ItemNotFoundInBasketException(productId.ToString());
        if (existingItems != null)
        {
            _items.Remove(existingItems);
        }
    }
}
