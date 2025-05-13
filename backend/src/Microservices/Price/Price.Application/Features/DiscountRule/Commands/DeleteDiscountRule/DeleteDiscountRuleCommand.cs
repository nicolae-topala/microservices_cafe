using Shared.Abstractions.Messaging.ResultType;

namespace Price.Application.Features.DiscountRule.Commands.DeleteDiscountRule;

public record DeleteDiscountRuleCommand(Guid DiscountRuleId) : IResultCommand;
