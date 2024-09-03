using Catalog.Data;
using Catalog.Products.Dtos;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Shared.CQRS;

namespace Catalog.Products.Features.GetProducts;

public sealed record GetProductsQuery() : IQuery<GetProductsResults>;

public sealed record GetProductsResults(IEnumerable<ProductDto> Products);
public sealed class GetProductsHandler(CatalogDbContext dbContext) : IQueryHandler<GetProductsQuery, GetProductsResults>
{
    public async Task<GetProductsResults> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await dbContext.Products
                     .AsNoTracking()
                     .OrderBy(x => x.Name)
                     .ToListAsync(cancellationToken: cancellationToken);

        var productsDto = products.Adapt<List<ProductDto>>();

        return new(productsDto);
    }
}