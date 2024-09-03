using Catalog.Data;
using Catalog.Products.Dtos;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Shared.CQRS;

namespace Catalog.Products.Features.GetProductById;

public sealed record GetProductByIdQuery(Guid ProductId) : IQuery<GetProductByIdResponse>;

public sealed record GetProductByIdResponse(ProductDto Product);

public class GetProductByIdHandler(CatalogDbContext dbContext) : IQueryHandler<GetProductByIdQuery, GetProductByIdResponse>
{
    public async Task<GetProductByIdResponse> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await dbContext.Products
                    .AsNoTracking()
                    .SingleOrDefaultAsync(x => x.Id == request.ProductId, cancellationToken)
                    ?? throw new Exception($"Product Was Not Found: {request.ProductId}");

        var productDto = product.Adapt<ProductDto>();

        return new(productDto);
    }
}