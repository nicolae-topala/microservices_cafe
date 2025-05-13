using Shared.Abstractions.Messaging;
using DiscountRuleDomain = Price.Domain.Entities.DiscountRule;

namespace Price.Application.Features.DiscountRule.Queries.GetDiscountRules;

public record GetDiscountRulesQuery : IQuery<IQueryable<DiscountRuleDomain>>;
