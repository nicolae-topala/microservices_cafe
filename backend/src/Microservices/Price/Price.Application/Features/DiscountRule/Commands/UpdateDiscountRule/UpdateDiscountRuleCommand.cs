using Price.Shared.DTOs.DiscountRule;
using Shared.Abstractions.Messaging.ResultType;

namespace Price.Application.Features.DiscountRule.Commands.UpdateDiscountRule;

public record UpdateDiscountRuleCommand(UpdateDiscountRuleDto DiscountRule) : IResultCommand;
