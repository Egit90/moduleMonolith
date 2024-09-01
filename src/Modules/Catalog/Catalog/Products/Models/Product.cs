using Shared.DDD;

namespace Catalog.Products.Models;

public sealed class Product : Entity<Guid>
{
    public string Name { get; private set; } = default!;
    public List<string> Category { get; private set; } = [];
    public string Description { get; private set; } = default!;
    public string ImageFile { get; private set; } = default!;
    public decimal Price { get; private set; }

    public static Product Create(Guid id, string name, List<string> category, string description, string imageFile, decimal price)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(price);

        return new Product
        {
            Name = name,
            Category = category,
            Description = description,
            ImageFile = imageFile,
            Price = price
        };
    }

    public void Update(string name, List<string> category, string description, string imageFile, decimal price)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(Price);

        Name = name;
        Category = category;
        Description = description;
        ImageFile = imageFile;
        Price = price;
    }
}