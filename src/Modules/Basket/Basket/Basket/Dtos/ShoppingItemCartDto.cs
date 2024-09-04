namespace Basket.Basket.Dtos;

public sealed record ShoppingCartItemDto(Guid Id, Guid ShoppingCartId, Guid ProductId, int Quantity, string Color, decimal price, string ProductName);

