using HotChocolate.Authorization;
using MediatR;
using Products.Application.Features.Categories.Commands.CreateCategory;
using Products.Application.Features.Categories.Commands.DeleteCategory;
using Products.Application.Features.Categories.Commands.EditCategory;
using Products.Domain.Entities;
using Products.Shared.DTOs;

namespace Products.API.Types.Mutations;

[MutationType]
[Authorize]
public class CategoriesMutations
{
    public async Task<Category> CreateCategory(ISender sender, CreateCategoryDto category)
    {
        var result = await sender.Send(new CreateCategoryCommand(category));
        return result.Value;
    }

    public async Task<Category> EditCategory(ISender sender, EditCategoryDto category)
    {
        var result = await sender.Send(new EditCategoryCommand(category));
        return result.Value;
    }

    public async Task<bool> DeleteCategory(ISender sender, Guid categoryId)
    {
        var result = await sender.Send(new DeleteCategoryCommand(categoryId));
        return result.Value;
    }
}
