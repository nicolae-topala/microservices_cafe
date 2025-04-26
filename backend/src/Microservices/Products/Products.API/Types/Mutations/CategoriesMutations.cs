using HotChocolate.Authorization;
using MediatR;
using Products.Application.Features.Categories.Commands.CreateCategory;
using Products.Application.Features.Categories.Commands.DeleteCategory;
using Products.Application.Features.Categories.Commands.EditCategory;
using Products.Domain.Entities;
using Products.Shared.DTOs.Category;
using Products.Shared.DTOs.Product;
using Shared.BuildingBlocks.Result;
using Shared.Helpers.Hotchocolate;

namespace Products.API.Types.Mutations;

[MutationType]
[Authorize]
public class CategoriesMutations
{
    [Error<ResultError>]
    public async Task<FieldResult<Category>> CreateCategory(ISender sender, CreateCategoryDto category) =>
        ResultHandler.HandleResponse(await sender.Send(new CreateCategoryCommand(category)));

    [Error<ResultError>]
    public async Task<FieldResult<Category>> EditCategory(ISender sender, EditCategoryDto category) =>
        ResultHandler.HandleResponse(await sender.Send(new EditCategoryCommand(category)));

    [Error<ResultError>]
    public async Task<FieldResult<bool>> DeleteCategory(ISender sender, Guid categoryId) =>
        ResultHandler.HandleResponse(await sender.Send(new DeleteCategoryCommand(categoryId)));
}
