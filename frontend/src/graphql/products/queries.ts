import { gql } from '@apollo/client';

export const SEARCH_PRODUCTS_BY_NAME = gql`
    query SearchProductsByName(
        $skip: Int!
        $take: Int!
        $productName: String!
    ) {
        searchProductsByName(
            skip: $skip
            take: $take
            productName: $productName
        ) {
            id
            name
            description
            categories {
                id
                name
            }
            variants {
                id
                priceAmount
                priceCurrency
            }
        }
    }
`;

export const SEARCH_PRODUCTS_WITH_FILTERS = gql`
    query SearchProductsWithFilters(
        $skip: Int!
        $take: Int!
        $productName: String
        $categoryIds: [UUID!]
        $minPrice: Float
        $maxPrice: Float
        $inStockOnly: Boolean
        $variantAttributesIds: [UUID!]
        $sortBy: SortBy!
        $sortOrder: SortOrder!
    ) {
        searchProductsWithFilters(
            skip: $skip
            take: $take
            productName: $productName
            categoryIds: $categoryIds
            minPrice: $minPrice
            maxPrice: $maxPrice
            inStockOnly: $inStockOnly
            variantAttributesIds: $variantAttributesIds
            sortBy: $sortBy
            sortOrder: $sortOrder
        ) {
            products {
                id
                name
                description
                isInStock
                categories {
                    id
                    name
                }
                variants {
                    id
                    priceAmount
                    priceCurrency
                }
            }
            filters {
                categories {
                    id
                    name
                    count
                }
                priceRange {
                    minPrice
                    maxPrice
                    buckets {
                        from
                        to
                        count
                        label
                    }
                }
                attributes {
                    id
                    attributeName
                    values {
                        value
                        count
                        unitsOfMeasure
                    }
                }
            }
            totalCount
        }
    }
`;
