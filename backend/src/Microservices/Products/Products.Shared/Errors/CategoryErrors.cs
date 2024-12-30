using Shared.BuildingBlocks.Result;

namespace Products.Shared.Errors;

public static class CategoryErrors
{
    public static readonly ResultError CategoryNotFound = new("Category.NotFound", "Category nout found!");
    public static readonly ResultError NullSubcategoryValue = new("Category.Subcategory.NullValue", "Subcategory cannot be null");
    public static readonly ResultError SelfPointing = new("Category.Subcategory.SelfPointing", "Category cannot be a subcategory of itself");
    public static readonly ResultError SubCategoryNotFound = new("Category.Subcategory.NotFound", "Sub Category nout found!");
    public static readonly ResultError AlreadyAssignedToParent = new("Category.Subcategory.AlreadyAssignedToParent", "Category is already assigned");
}