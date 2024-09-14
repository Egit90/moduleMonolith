using System.Text.Json;
using System.Text.Json.Serialization;
using Basket.Basket.Models;
using Basket.Data.JsonConverters;
using Microsoft.Extensions.Caching.Distributed;

namespace Basket.Repository;

public class CashedBasketRepository(IBasketRepository basketRepository, IDistributedCache cache) : IBasketRepository
{
    private readonly JsonSerializerOptions _options = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        Converters = { new ShoppingCartConverter(), new ShoppingCartItemConverter() }
    };

    public async Task<ShoppingCart> CreateBasket(ShoppingCart basket, CancellationToken cancellationToken = default)
    {
        var basket2 = await basketRepository.CreateBasket(basket, cancellationToken);

        await cache.SetStringAsync(basket2.UserName, JsonSerializer.Serialize(basket, _options), token: cancellationToken);
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
            // Deserialize
            return JsonSerializer.Deserialize<ShoppingCart>(cashedBasket!, _options)!;
        }

        var basket = await basketRepository.GetBasket(UserName, asNoTracking, cancellationToken);
        await cache.SetStringAsync(UserName, JsonSerializer.Serialize(basket, _options), token: cancellationToken);
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
