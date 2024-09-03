using Catalog.Products.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Catalog.Products.EventHandlers;

public sealed class ProductPriceChangeEventHandler(ILogger<ProductPriceChangeEventHandler> logger) : INotificationHandler<ProductPriceChangeEvent>
{
    public Task Handle(ProductPriceChangeEvent notification, CancellationToken cancellationToken)
    {
        // Todo Publish Product Price Integration Event
        logger.LogInformation("Domain Event Handled: {DomainEvent}", notification.GetType().Name);
        return Task.CompletedTask;
    }
}