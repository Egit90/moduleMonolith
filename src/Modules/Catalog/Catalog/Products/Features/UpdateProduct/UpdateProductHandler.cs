using Catalog.Data;
using Catalog.Products.Dtos;
using Catalog.Products.Models;
using Shared.CQRS;

namespace Catalog.Products.Features.UpdateProduct;

public sealed record UpdateProductCommand(ProductDto Product) : ICommand<UpdateProductResults>;
public sealed record UpdateProductResults(bool IsSuccess);

public sealed class UpdateProductHandler(CatalogDbContext dbContext) : ICommandHandler<UpdateProductCommand, UpdateProductResults>
{
    public async Task<UpdateProductResults> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await dbContext.Products.FindAsync([request.Product.Id], cancellationToken: cancellationToken)
                    ?? throw new Exception($"Product Was Not Found: {request.Product.Name}");

        UpdateProduct(product, request.Product);

        dbContext.Products.Update(product);

        await dbContext.SaveChangesAsync(cancellationToken);

        return new UpdateProductResults(true);

    }

    private void UpdateProduct(Product product, ProductDto Dto) => product.Update(Dto.Name, Dto.Category, Dto.Description, Dto.ImageFile, Dto.Price);
}