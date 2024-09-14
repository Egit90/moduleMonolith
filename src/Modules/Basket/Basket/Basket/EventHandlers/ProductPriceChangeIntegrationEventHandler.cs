using Basket.Basket.Features.UpdateItemPriceInBasket;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared.Messaging.Events;

namespace Basket.Basket.EventHandlers;

public sealed class ProductPriceChangeIntegrationEventHandler
(ILogger<ProductPriceChangeIntegrationEventHandler> logger, ISender sender) : IConsumer<ProductPriceChangeIntegrationEvent>
{
    public async Task Consume(ConsumeContext<ProductPriceChangeIntegrationEvent> context)
    {
        logger.LogInformation("Integration Event Handled: {IntegrationEvent}", context.Message.GetType().Name);
        // Find basket Items with product id and update item price

        // mediator
        var command = new UpdateItemPriceInBasketCommand(context.Message.ProductId, context.Message.Price);
        var res = await sender.Send(command);

        if (!res.IsSuccess)
        {
            logger.LogError("Error Updating a price in basket for product id: {productOT}", context.Message.ProductId);
            return;
        }
        logger.LogError("Price for Product id : {productOT} was updated in Basket", context.Message.ProductId);
    }
}
