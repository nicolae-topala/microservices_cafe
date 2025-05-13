using Products.Shared.Errors;
using Shared.BuildingBlocks.Result;
using Shared.Primitives;

namespace Products.Domain.Entities;

public sealed class Category : AggregateRoot
{
    private readonly List<Category> _subCategories = [];
    private readonly List<Product> _products = [];

    public string Name { get; private set; }
    public Guid? ParentCategoryId { get; private set; }
    public Category? ParentCategory { get; private set; }
    public IReadOnlyCollection<Product> Products => _products;
    public IReadOnlyCollection<Category>? SubCategories => _subCategories;

    private Category() { }

    private Category(string name, Category? parentCategory = null) : base()
    {
        Name = name;
        ParentCategoryId = parentCategory.Id;
    }

    public static Result<Category> Create(string name, Category? parentCategory = null)
    {
        var category = new Category(name, parentCategory);
        parentCategory?.AddSubCategory(category);

        return Result.Success(category);
    }

    public Result<Category> Edit(string? name = null, Category? parentCategory = null, List<Category>? subCategories = null)
    {
        if (name is not null)
        {
            Name = name;
        }

        if (parentCategory is not null) 
        { 
            var resultChange = ChangeParentCategory(parentCategory);
            if (resultChange.IsFailure)
            {
                return Result.Failure<Category>(resultChange.Error);
            }
        }

        return Result.Success(this);
    }

    public Result ChangeParentCategory(Category newParentCategory)
    {
        if(newParentCategory == this)
        {
            return Result.Failure(CategoryErrors.SelfPointing);
        }

        ParentCategory?.RemoveSubCategory(this);
        newParentCategory.AddSubCategory(this);
        return Result.Success();
    }

    public Result RemoveParentCategory()
    {
        ParentCategory = null;
        return Result.Success();
    }

    public Result AddSubCategory(Category subCategory)
    {
        if (subCategory == this)
        {
            return Result.Failure(CategoryErrors.SelfPointing);
        }

        if (_subCategories.Contains(subCategory))
        {
            return Result.Failure(CategoryErrors.AlreadyAssignedToParent);
        }

        _subCategories.Add(subCategory);
        subCategory.SetParentCategory(this);

        return Result.Success();
    }

    public Result RemoveSubCategory(Category subCategory)
    {
        var removeCategory = _subCategories.Remove(subCategory);

        if (!removeCategory)
            return Result.Failure(CategoryErrors.SubCategoryNotFound);

        subCategory.SetParentCategory(null);
        return Result.Success();
    }

    public Result RemoveAllSubCategories()
    {
        foreach (var subCategory in _subCategories)
        {
            var resultRemoving = RemoveSubCategory(subCategory);
            if (resultRemoving.IsFailure)
            {
                return Result.Failure(resultRemoving.Error);
            }
        }
        return Result.Success();
    }

    public Result AddRangeSubCategories(IEnumerable<Category> newSubCategories)
    {
        if (newSubCategories.Any(x => x == this))
        {
            return Result.Failure(CategoryErrors.SelfPointing);
        }

        foreach (var newSubCategory in newSubCategories)
        {
            var resultAdding = AddSubCategory(newSubCategory);
            if (resultAdding.IsFailure)
            {
                return Result.Failure(resultAdding.Error);
            }
        }
        return Result.Success();
    }

    public Result RemoveRangeSubCategories(IEnumerable<Category> subCategories)
    {
        foreach (var subCategory in subCategories)
        {
            var resultRemoving = RemoveSubCategory(subCategory);
            if (resultRemoving.IsFailure)
            {
                return Result.Failure(resultRemoving.Error);
            }
        }
        return Result.Success();
    }

    private void SetParentCategory(Category? parent)
    {
        ParentCategory = parent;
    }
}
