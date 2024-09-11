using System.Text.Json;
using Basket.Basket.Models;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;

namespace Basket.Repository;

public class CashedBasketRepository(IBasketRepository basketRepository, IDistributedCache cache) : IBasketRepository
{
    public async Task<ShoppingCart> CreateBasket(ShoppingCart basket, CancellationToken cancellationToken = default)
    {
        var basket2 = await basketRepository.CreateBasket(basket, cancellationToken);

        await cache.SetStringAsync(basket2.UserName, JsonSerializer.Serialize(basket), token: cancellationToken);
        return basket2;
    }

    public async Task<bool> DeleteBasket(string UserName, CancellationToken cancellationToken = default)
    {
        await basketRepository.DeleteBasket(UserName, cancellationToken);
        await cache.RemoveAsync(UserName, cancellationToken);

        return true;
    }

    public async Task<ShoppingCart> GetBasket(string UserName, bool asNoTracking = true, CancellationToken cancellationToken = default)
    {
        if (!asNoTracking)
        {
            return await basketRepository.GetBasket(UserName, asNoTracking, cancellationToken);
        }

        var cashedBasket = await cache.GetStringAsync(UserName, cancellationToken);

        if (!string.IsNullOrEmpty(cashedBasket))
        {
            return JsonSerializer.Deserialize<ShoppingCart>(cashedBasket!)!;
        }

        var basket = await basketRepository.GetBasket(UserName, asNoTracking, cancellationToken);
        await cache.SetStringAsync(UserName, JsonSerializer.Serialize(basket), token: cancellationToken);
        return basket;
    }

    public async Task<int> SaveChangesAsync(string? UserName = null, CancellationToken cancellationToken = default)
    {
        var res = await basketRepository.SaveChangesAsync(UserName, cancellationToken);
        // Clear Cache

        if (!string.IsNullOrEmpty(UserName))
        {
            await cache.RemoveAsync(UserName, cancellationToken);
        }

        return res;
    }
}
