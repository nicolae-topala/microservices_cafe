using Products.Shared.Errors;
using Shared.BuildingBlocks.Result;
using Shared.Primitives;

namespace Products.Domain.Entities;

public sealed class Category : BaseEntity
{
    private readonly List<Category> _subCategories = [];
    private readonly List<Product> _products = [];

    public string Name { get; private set; }
    public Category? ParentCategory { get; private set; }
    public IReadOnlyCollection<Product> Products => _products;
    public IReadOnlyCollection<Category>? SubCategories => _subCategories;

    private Category() { }

    private Category(string name, Category? parentCategory = null) : base()
    {
        Name = name;
        ParentCategory = parentCategory;
    }

    public static Result<Category> Create(string name, Category? parentCategory = null)
    {
        var category = new Category(name, parentCategory);

        if (parentCategory is not null)
        {
            parentCategory.AddSubCategory(category);
        }

        return Result.Success(category);
    }

    public Result<Category> Edit(string name)
    {
        Name = name;

        return Result.Success(this);
    }

    public Result<Category> AddSubCategory(Category subCategory)
    {
        if (subCategory is null)
        {
            return Result.Failure<Category>(CategoryErrors.NullSubcategoryValue);
        }

        if (subCategory == this)
        {
            return Result.Failure<Category>(CategoryErrors.SelfPointing);
        }

        _subCategories.Add(subCategory);

        return Result.Success(this);
    }

    public Result<Category> RemoveSubCategory(Category subCategory)
    {
        if (subCategory is null)
        {
            return Result.Failure<Category>(CategoryErrors.NullSubcategoryValue);
        }

        _subCategories.Remove(subCategory);

        return Result.Success(this);
    }
}
