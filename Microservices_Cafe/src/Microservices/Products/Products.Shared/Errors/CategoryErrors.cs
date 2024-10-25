using Shared.BuildingBlocks.Result;

namespace Products.Shared.Errors;

public static class CategoryErrors
{
    public static readonly ResultError NullSubcategoryValue = new("Category.Subcategory.NullValue", "Subcategory cannot be null");
    public static readonly ResultError SelfPointing = new("Category.Subcategory.SelfPointing", "Category cannot be a subcategory of itself");
}