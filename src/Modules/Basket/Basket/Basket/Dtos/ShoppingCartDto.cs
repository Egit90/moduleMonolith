using Basket.Basket.Models;

namespace Basket.Basket.Dtos;

public sealed record ShoppingCartDto(Guid Id, string UserName, List<ShoppingCartItemDto> Items);

