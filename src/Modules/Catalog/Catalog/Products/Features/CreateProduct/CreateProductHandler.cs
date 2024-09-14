using Catalog.Data;
using Catalog.Products.Models;
using FluentValidation;

namespace Catalog.Products.Features.CreateProduct;

public sealed record CreateProductCommand(ProductDto Product) : ICommand<CreateProductResult>;
public sealed record CreateProductResult(Guid Id);


public sealed class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Product.Name).NotEmpty().WithMessage("Name is required");
        RuleFor(x => x.Product.Category).NotEmpty().WithMessage("Category is required");
        RuleFor(x => x.Product.ImageFile).NotEmpty().WithMessage("ImageFile is required");
        RuleFor(x => x.Product.Price).GreaterThan(0).WithMessage("Price must be greater than 0");
    }
}

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