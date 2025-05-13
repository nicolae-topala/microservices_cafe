using HotChocolate.Authorization;
using MediatR;
using Price.Application.Features.DiscountRule.Commands.CreateDiscountRule;
using Price.Application.Features.DiscountRule.Commands.DeleteDiscountRule;
using Price.Application.Features.DiscountRule.Commands.UpdateDiscountRule;
using Price.Domain.Entities;
using Price.Shared.DTOs.DiscountRule;
using Shared.BuildingBlocks.Result;
using Shared.Helpers.Hotchocolate;

namespace Price.API.Types.Mutations;

[MutationType]
[Authorize]
public class DiscountRuleMutations
{
    [Error<ResultError>]
    public async Task<FieldResult<DiscountRule>> CreateDiscountRule(ISender sender, CreateDiscountRuleDto discountRuleDto, CancellationToken cancellationToken) =>
        ResultHandler.HandleResponse(await sender.Send(new CreateDiscountRuleCommand(discountRuleDto), cancellationToken));

    [Error<ResultError>]
    public async Task<FieldResult<bool>> UpdateDiscountRule(ISender sender, UpdateDiscountRuleDto discountRuleDto, CancellationToken cancellationToken) =>
        ResultHandler.HandleResponse(await sender.Send(new UpdateDiscountRuleCommand(discountRuleDto), cancellationToken));

    [Error<ResultError>]
    public async Task<FieldResult<bool>> DeleteDiscountRule(ISender sender, Guid id, CancellationToken cancellationToken) =>
        ResultHandler.HandleResponse(await sender.Send(new DeleteDiscountRuleCommand(id), cancellationToken));
}
