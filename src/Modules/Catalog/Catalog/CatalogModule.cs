using Catalog.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Catalog;

public static class CatalogModule
{
    public static IServiceCollection AddCatalogModule(this IServiceCollection services, IConfiguration Configuration)
    {
        var connectionString = Configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<CatalogDb>(opt => opt.UseNpgsql(connectionString));
        return services;
    }
    public static IApplicationBuilder UseCatalogModule(this IApplicationBuilder app)
    {
        return app;
    }
}
