using Shared.Exceptions;

namespace Basket.Basket.Exceptions;
public sealed class BasketNotFoundException(string UserName) : NotFoundException("Basket", UserName)
{
}