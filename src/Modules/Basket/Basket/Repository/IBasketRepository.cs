using Basket.Basket.Models;

namespace Basket.Repository;

public interface IBasketRepository
{
    Task<ShoppingCart> GetBasket(string UserName, bool asNoTracking = true, CancellationToken cancellationToken = default);
    Task<ShoppingCart> CreateBasket(ShoppingCart basket, CancellationToken cancellationToken = default);
    Task<bool> DeleteBasket(string UserName, CancellationToken cancellationToken = default);
    Task<int> SaveChangesAsync(string? UserName = null, CancellationToken cancellationToken = default);

}
