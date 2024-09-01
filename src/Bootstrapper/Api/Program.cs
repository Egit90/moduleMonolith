using Basket;
using Catalog;

var builder = WebApplication.CreateBuilder(args);

// Add Services
builder.Services
       .AddCatalogModule(builder.Configuration)
       .AddBasketModule(builder.Configuration)
       .AddBasketModule(builder.Configuration);


var app = builder.Build();

// Configure the HTTP request pipeline

app.Run();
