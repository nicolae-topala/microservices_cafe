using Price.Shared.DTOs.DiscountRule;
using Shared.Abstractions.Messaging.ResultType;
using DiscountRuleDomain = Price.Domain.Entities.DiscountRule;

namespace Price.Application.Features.DiscountRule.Commands.CreateDiscountRule;

public record CreateDiscountRuleCommand(CreateDiscountRuleDto DiscountRule) : IResultCommand<DiscountRuleDomain>;
