using Catalog.Data;
using Catalog.Products.Dtos;
using Catalog.Products.Models;
using MediatR;
using Shared.CQRS;

namespace Catalog.Products.Features.CreateProduct;

public sealed record CreateProductCommand(ProductDto Product) : ICommand<CreateProductResult>;
public sealed record CreateProductResult(Guid Id);


public class CreateProductHandler(CatalogDbContext dbContext) : ICommandHandler<CreateProductCommand, CreateProductResult>
{
    public async Task<CreateProductResult> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = CreateNewProduct(request.Product);
        dbContext.Add(product);

        await dbContext.SaveChangesAsync(cancellationToken);

        return new(product.Id);
    }


    private Product CreateNewProduct(ProductDto dto) => Product.Create(Guid.NewGuid(), dto.Name, dto.Category, dto.Description, dto.ImageFile, dto.Price);
}