using Catalog.Data;
using Catalog.Products.Exceptions;
using Catalog.Products.Models;
using FluentValidation;

namespace Catalog.Products.Features.UpdateProduct;

public sealed record UpdateProductCommand(ProductDto Product) : ICommand<UpdateProductResults>;
public sealed record UpdateProductResults(bool IsSuccess);

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(x => x.Product.Id).NotEmpty().WithMessage("Id is required");
        RuleFor(x => x.Product.Name).NotEmpty().WithMessage("Name is required");
        RuleFor(x => x.Product.Price).GreaterThan(0).WithMessage("Price must be greater than 0");
    }
}
public sealed class UpdateProductHandler(CatalogDbContext dbContext) : ICommandHandler<UpdateProductCommand, UpdateProductResults>
{
    public async Task<UpdateProductResults> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await dbContext.Products.FindAsync([request.Product.Id], cancellationToken: cancellationToken)
                    ?? throw new ProductNotFoundException(request.Product.Id);

        UpdateProduct(product, request.Product);

        dbContext.Products.Update(product);

        await dbContext.SaveChangesAsync(cancellationToken);

        return new UpdateProductResults(true);

    }

    private void UpdateProduct(Product product, ProductDto Dto) => product.Update(Dto.Name, Dto.Category, Dto.Description, Dto.ImageFile, Dto.Price);
}