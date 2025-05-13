using Price.Application.Abstractions;
using DiscountRuleDomain = Price.Domain.Entities.DiscountRule;
using Shared.Abstractions.Messaging.ResultType;
using Shared.BuildingBlocks.Result;
using Microsoft.EntityFrameworkCore;

namespace Price.Application.Features.DiscountRule.Commands.CreateDiscountRule;

public class CreateDiscountRuleCommandHandler(IPriceDbContext dbContext)
    : IResultCommandHandler<CreateDiscountRuleCommand, DiscountRuleDomain>
{
    public async Task<Result<DiscountRuleDomain>> Handle(CreateDiscountRuleCommand request, CancellationToken cancellationToken)
    {
        var channel = await dbContext.Channels
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.DiscountRule.ChannelId, cancellationToken);

        if (channel is null)
        {
            return Result.Failure<DiscountRuleDomain>(new ResultError("DiscountRule.InvalidChannel", "Channel not found"));
        }

        var discountRuleResult = DiscountRuleDomain.Create(
        request.DiscountRule.DiscountPercentage,
        request.DiscountRule.Condition,
        request.DiscountRule.EffectiveFrom,
        request.DiscountRule.EffectiveTo,
        channel,
        request.DiscountRule.ProductVariantId,
        request.DiscountRule.ProductCategoryId);

        if (discountRuleResult.IsFailure)
        {
            return Result.Failure<DiscountRuleDomain>(discountRuleResult.Error);
        }

        await dbContext.DiscountRules
            .AddAsync(discountRuleResult.Value, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);
        return Result.Success(discountRuleResult.Value);
    }
}
