using MapsterMapper;
using Products.Application.Abstractions;
using Products.Shared.DTOs;
using Shared.Abstractions.Messaging;
using Shared.BuildingBlocks.Result;
using Microsoft.EntityFrameworkCore;

namespace Products.Application.Features.Products.Queries.GetProductById;

public class GetProductByIdHandler(
    IProductsDbContext dbContext,
    IMapper mapper) 
    : IQueryHandler<GetProductByIdQuery, ProductDto>
{
    public async Task<Result<ProductDto>> Handle(
        GetProductByIdQuery request, 
        CancellationToken cancellationToken)
    {
        var product = await dbContext.Products
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == request.Id, 
                cancellationToken)
            .ConfigureAwait(false);

        if (product is null)
        {
            return Result.Failure<ProductDto>(new Error("", ""));
        }

        var result = mapper.Map<ProductDto>(product);
        return Result.Create(result);
    }
}

