namespace Shared.BuildingBlocks.Elasticsearch;

public static class ElasticsearchConstants
{
    public static class FieldSuffixes
    {
        public const string Keyword = "keyword";
        public const string Text = "text";
        public const string Raw = "raw";
    }

    public static class Boost
    {
        public const float ExactPhrase = 10.0f;
        public const float PartialMatch = 8.0f;
        public const float StandardMatch = 5.0f;
        public const float DescriptionMatch = 2.0f;
        public const float FuzzyMatch = 1.0f;
    }

    public static class ElasticsearchAggregationKeys
    {
        public const string Categories = "categories";
        public const string CategoryIds = "category_ids";
        public const string CategoryNames = "category_names";

        public const string PriceStats = "price_stats";
        public const string MinPrice = "min_price";
        public const string MaxPrice = "max_price";

        public const string VariantAttributes = "variant_attributes";
        public const string Attributes = "attributes";
        public const string AttributeIds = "attribute_ids";
        public const string AttributeNames = "attribute_names";
        public const string AttributeValues = "attribute_values";
    }
}