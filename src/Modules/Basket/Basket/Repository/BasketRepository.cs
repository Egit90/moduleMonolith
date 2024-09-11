using Basket.Basket.Exceptions;
using Basket.Basket.Models;
using Basket.Data;
using Microsoft.EntityFrameworkCore;

namespace Basket.Repository;

public class BasketRepository(BasketDbContext dbContext) : IBasketRepository
{
    public async Task<ShoppingCart> CreateBasket(ShoppingCart basket, CancellationToken cancellationToken = default)
    {
        dbContext.ShoppingCarts.Add(basket);
        await dbContext.SaveChangesAsync(cancellationToken);
        return basket;
    }

    public async Task<bool> DeleteBasket(string UserName, CancellationToken cancellationToken = default)
    {
        var basket = await GetBasket(UserName, false, cancellationToken)
                    ?? throw new BasketNotFoundException(UserName);

        dbContext.ShoppingCarts.Remove(basket);
        await dbContext.SaveChangesAsync(cancellationToken);

        return true;

    }

    public async Task<ShoppingCart> GetBasket(string UserName, bool asNoTracking = true, CancellationToken cancellationToken = default)
    {
        var query = dbContext.ShoppingCarts
                    .Include(x => x.Items)
                    .Where(x => x.UserName == UserName);

        if (asNoTracking) query.AsNoTracking();

        var basket = await query.SingleOrDefaultAsync(cancellationToken)
                        ?? throw new BasketNotFoundException(UserName);

        return basket;
    }

    public async Task<int> SaveChangesAsync(string? UserName = null, CancellationToken cancellationToken = default)
    {
        return await dbContext.SaveChangesAsync(cancellationToken);
    }
}
