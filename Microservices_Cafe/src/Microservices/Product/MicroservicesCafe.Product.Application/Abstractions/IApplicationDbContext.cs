using Microsoft.EntityFrameworkCore;
using MicroservicesCafe.Product.Domain.Entities;
using Products = MicroservicesCafe.Product.Domain.Entities.Product;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace MicroservicesCafe.Product.Application.Abstractions;

public interface IApplicationDbContext
{
    DbSet<Products> Products { get; }
    DbSet<Category> Categories { get; }
    DatabaseFacade Database { get; }
}
