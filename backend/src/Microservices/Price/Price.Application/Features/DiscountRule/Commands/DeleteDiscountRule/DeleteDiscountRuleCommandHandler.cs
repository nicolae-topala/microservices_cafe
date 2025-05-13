using Microsoft.EntityFrameworkCore;
using Price.Application.Abstractions;
using Shared.Abstractions.Messaging.ResultType;
using Shared.BuildingBlocks.Result;

namespace Price.Application.Features.DiscountRule.Commands.DeleteDiscountRule;

public class DeleteDiscountRuleCommandHandler(IPriceDbContext dbContext)
    : IResultCommandHandler<DeleteDiscountRuleCommand>
{
    public async Task<Result> Handle(DeleteDiscountRuleCommand request, CancellationToken cancellationToken)
    {
        var discountRule = await dbContext.DiscountRules
            .FirstOrDefaultAsync(x => x.Id == request.DiscountRuleId, cancellationToken);

        if (discountRule is null)
        {
            return Result.Failure(new ResultError("DiscountRule.NotFound", "Discount rule not found"));
        }

        dbContext.DiscountRules.Remove(discountRule);

        await dbContext.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
