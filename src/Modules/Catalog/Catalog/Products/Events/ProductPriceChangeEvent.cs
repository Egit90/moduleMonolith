using Catalog.Products.Models;
using Shared.DDD;

namespace Catalog.Products.Events;

public sealed record ProductPriceChangeEvent(Product Product) : IDomainEvent;