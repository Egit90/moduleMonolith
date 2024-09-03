using Catalog.Data;
using Catalog.Products.Dtos;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Shared.CQRS;

namespace Catalog.Products.Features.GetProductByCategory;

public sealed record GetProductByCategoryQuery(string Category) : IQuery<GetProductByCategoryResults>;

public sealed record GetProductByCategoryResults(IEnumerable<ProductDto> Products);

public class GetProductByCategoryHandler(CatalogDbContext dbContext) : IQueryHandler<GetProductByCategoryQuery, GetProductByCategoryResults>
{
    public async Task<GetProductByCategoryResults> Handle(GetProductByCategoryQuery request, CancellationToken cancellationToken)
    {
        var products = await dbContext.Products
                     .AsNoTracking()
                     .Where(x => x.Category.Contains(request.Category))
                     .OrderBy(x => x.Name)
                     .ToListAsync(cancellationToken: cancellationToken);

        var ProductDto = products.Adapt<List<ProductDto>>();

        return new(ProductDto);

    }
}