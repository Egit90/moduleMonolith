using Shared.Exceptions;

namespace Basket.Basket.Exceptions;

public sealed class ItemNotFoundInBasketException(string Item) : NotFoundException("Item", Item)
{
}