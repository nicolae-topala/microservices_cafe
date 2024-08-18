using MicroservicesCafe.Products.Application.Abstractions;
using MicroservicesCafe.Products.Domain.Entities;
using MicroservicesCafe.Shared.Abstractions.Messaging;
using MicroservicesCafe.Shared.BuildingBlocks.Result;
using MicroservicesCafe.Shared.Primitives;

namespace MicroservicesCafe.Products.Application.Features.Products.Commands.CreateProduct;

public class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, EntityCreatedResponse>
{
    private readonly IProductsDbContext _dbContext;

    public CreateProductCommandHandler(IProductsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<EntityCreatedResponse>> Handle(CreateProductCommand request, 
        CancellationToken cancellationToken)
    {
        var productResult = Product.Create(request.Name, request.Description, request.Price, request.Type, request.CategoryId);

        if (productResult.IsFailure) 
        {
            return Result.Failure<EntityCreatedResponse>(productResult.Error);
        }

        await _dbContext.Products.AddAsync(productResult.Value, cancellationToken).ConfigureAwait(false);

        return Result.Success(new EntityCreatedResponse(productResult.Value.Id));
    }
}

