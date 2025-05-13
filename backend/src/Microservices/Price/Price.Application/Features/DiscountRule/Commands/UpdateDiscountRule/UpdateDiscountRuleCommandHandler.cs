using Microsoft.EntityFrameworkCore;
using Price.Application.Abstractions;
using Price.Domain.Entities;
using Shared.Abstractions.Messaging.ResultType;
using Shared.BuildingBlocks.Result;

namespace Price.Application.Features.DiscountRule.Commands.UpdateDiscountRule;
public class UpdateDiscountRuleCommandHandler(IPriceDbContext dbContext)
    : IResultCommandHandler<UpdateDiscountRuleCommand>
{
    public async Task<Result> Handle(UpdateDiscountRuleCommand request, CancellationToken cancellationToken)
    {
        var discountRule = await dbContext.DiscountRules
            .FirstOrDefaultAsync(x => x.Id == request.DiscountRule.DiscountRuleId, cancellationToken);

        if (discountRule is null)
        {
            return Result.Failure(new ResultError("DiscountRule.NotFound", "Discount rule not found"));
        }

        Channel? channel = null;
        if (request.DiscountRule.ChannelId.HasValue)
        {
            channel = await dbContext.Channels
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == request.DiscountRule.ChannelId, cancellationToken);

            if (channel is null)
            {
                return Result.Failure(new ResultError("DiscountRule.InvalidChannel", "Channel not found"));
            }
        }

        var discountRuleResult = discountRule.Update(
            request.DiscountRule.DiscountPercentage,
            request.DiscountRule.Condition,
            request.DiscountRule.EffectiveFrom,
            request.DiscountRule.EffectiveTo,
            channel,
            request.DiscountRule.ProductVariantId,
            request.DiscountRule.ProductCategoryId);

        if (discountRuleResult.IsFailure)
        {
            return Result.Failure(discountRuleResult.Error);
        }

        await dbContext.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
