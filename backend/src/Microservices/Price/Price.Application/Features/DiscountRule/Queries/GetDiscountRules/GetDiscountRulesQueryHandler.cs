using Microsoft.EntityFrameworkCore;
using Price.Application.Abstractions;
using Shared.Abstractions.Messaging;
using DiscountRuleDomain = Price.Domain.Entities.DiscountRule;

namespace Price.Application.Features.DiscountRule.Queries.GetDiscountRules;

public class GetDiscountRulesQueryHandler(IPriceDbContext dbContext)
    : IQueryHandler<GetDiscountRulesQuery, IQueryable<DiscountRuleDomain>>
{
    public Task<IQueryable<DiscountRuleDomain>> Handle(GetDiscountRulesQuery request, CancellationToken cancellationToken) =>
        Task.FromResult(
            dbContext.DiscountRules
                .Include(dr => dr.Channel)
                .AsNoTracking());
}
