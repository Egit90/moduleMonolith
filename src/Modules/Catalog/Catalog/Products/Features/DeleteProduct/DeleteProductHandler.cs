using Catalog.Data;
using Shared.CQRS;

namespace Catalog.Products.Features.DeleteProduct;


public sealed record DeleteProductCommand(Guid ID) : ICommand<DeleteProductResponse>;
public sealed record DeleteProductResponse(bool IsSuccess);
public class DeleteProductHandler(CatalogDbContext dbContext) : ICommandHandler<DeleteProductCommand, DeleteProductResponse>
{
    public async Task<DeleteProductResponse> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
    {
        var product = await dbContext.Products.FindAsync([command.ID], cancellationToken: cancellationToken)
            ?? throw new Exception($"Product Was Not Found: {command.ID}");

        dbContext.Products.Remove(product);

        await dbContext.SaveChangesAsync(cancellationToken);

        return new(true);
    }
}