using Catalog.Data;
using Catalog.Products.Dtos;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Shared.CQRS;

namespace Catalog.Products.Features.GetProducts;

public sealed record GetProductQuery() : IQuery<GetProductResponse>;

public sealed record GetProductResponse(IEnumerable<ProductDto> Products);
public sealed class GetProductsHandler(CatalogDbContext dbContext) : IQueryHandler<GetProductQuery, GetProductResponse>
{
    public async Task<GetProductResponse> Handle(GetProductQuery request, CancellationToken cancellationToken)
    {
        var products = await dbContext.Products
                     .AsNoTracking()
                     .OrderBy(x => x.Name)
                     .ToListAsync(cancellationToken: cancellationToken);

        var productsDto = products.Adapt<List<ProductDto>>();

        return new(productsDto);
    }
}