using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared.Messaging.Events;

namespace Basket.Basket.EventHandlers;

public sealed class ProductPriceChangeIntegrationEventHandler
(ILogger<ProductPriceChangeIntegrationEventHandler> logger, ISender sender) : IConsumer<ProductPriceChangeIntegrationEvent>
{
    public Task Consume(ConsumeContext<ProductPriceChangeIntegrationEvent> context)
    {
        logger.LogInformation("Integration Event Handled: {IntegrationEvent}", context.Message.GetType().Name);
        // Find basket Items with product id and update item price

        // mediator
        return Task.CompletedTask;
    }
}
