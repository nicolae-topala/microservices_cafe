using Microsoft.EntityFrameworkCore;
using MicroservicesCafe.Products.Domain.Entities;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace MicroservicesCafe.Products.Application.Abstractions;

public interface IProductsDbContext
{
    DbSet<Product> Products { get; }
    DbSet<Category> Categories { get; }
    DatabaseFacade Database { get; }
}
