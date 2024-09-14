using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared.Messaging.Events;

namespace Catalog.Products.EventHandlers;

public sealed class ProductCreatedEventHandler(ILogger<ProductCreatedEventHandler> logger, IBus bus) : INotificationHandler<ProductCreatedEvent>
{
    public async Task Handle(ProductCreatedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Domain Event Handled: {DomainEvent}", notification.GetType().Name);

        // publish Integration Events
        ProductPriceChangeIntegrationEvent IntegrationEvent = new()
        {
            ProductId = notification.Product.Id,
            Name = notification.Product.Name,
            Category = notification.Product.Category,
            Description = notification.Product.Description,
            ImageFile = notification.Product.ImageFile,
            Price = notification.Product.Price,
        };

        await bus.Publish(IntegrationEvent, cancellationToken);
    }
}
