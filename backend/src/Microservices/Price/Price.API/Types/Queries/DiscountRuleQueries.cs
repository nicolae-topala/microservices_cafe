using MediatR;
using Price.Application.Features.DiscountRule.Queries.GetDiscountRules;
using Price.Domain.Entities;

namespace Price.API.Types.Queries;

[QueryType]
public class DiscountRuleQueries
{
    [UsePaging]
    [UseFiltering]
    [UseSorting]
    public Task<IQueryable<DiscountRule>> GetDiscountRules(ISender sender, CancellationToken cancellationToken) =>
        sender.Send(new GetDiscountRulesQuery(), cancellationToken);
}
