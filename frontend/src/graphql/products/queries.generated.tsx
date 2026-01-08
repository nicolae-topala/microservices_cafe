import * as Types from '../../generated/types';

import { gql } from '@apollo/client';
import * as Apollo from '@apollo/client';
const defaultOptions = {} as const;
export type SearchProductsByNameQueryVariables = Types.Exact<{
  skip: Types.Scalars['Int']['input'];
  take: Types.Scalars['Int']['input'];
  productName: Types.Scalars['String']['input'];
}>;


export type SearchProductsByNameQuery = { __typename?: 'Query', searchProductsByName: Array<{ __typename?: 'ProductDocument', id: any, name: string, description: string, categories: Array<{ __typename?: 'CategoryDocument', id: any, name: string }>, variants: Array<{ __typename?: 'ProductVariantDocument', id: any, priceAmount: any, priceCurrency: string }> }> };

export type SearchProductsWithFiltersQueryVariables = Types.Exact<{
  skip: Types.Scalars['Int']['input'];
  take: Types.Scalars['Int']['input'];
  productName?: Types.InputMaybe<Types.Scalars['String']['input']>;
  categoryIds?: Types.InputMaybe<Array<Types.Scalars['UUID']['input']> | Types.Scalars['UUID']['input']>;
  minPrice?: Types.InputMaybe<Types.Scalars['Float']['input']>;
  maxPrice?: Types.InputMaybe<Types.Scalars['Float']['input']>;
  inStockOnly?: Types.InputMaybe<Types.Scalars['Boolean']['input']>;
  variantAttributesIds?: Types.InputMaybe<Array<Types.Scalars['UUID']['input']> | Types.Scalars['UUID']['input']>;
  sortBy: Types.SortBy;
  sortOrder: Types.SortOrder;
}>;


export type SearchProductsWithFiltersQuery = { __typename?: 'Query', searchProductsWithFilters: { __typename?: 'SearchProductsWithFiltersResponse', totalCount: any, products: Array<{ __typename?: 'ProductDocument', id: any, name: string, description: string, isInStock: boolean, categories: Array<{ __typename?: 'CategoryDocument', id: any, name: string }>, variants: Array<{ __typename?: 'ProductVariantDocument', id: any, priceAmount: any, priceCurrency: string }> }>, filters?: { __typename?: 'SearchFilters', categories: Array<{ __typename?: 'CategoryFilter', id: any, name: string, count: number }>, priceRange: { __typename?: 'PriceRangeFilter', minPrice?: number | null, maxPrice?: number | null, buckets: Array<{ __typename?: 'PriceRangeBucket', from: any, to: any, count: number, label: string }> }, attributes: Array<{ __typename?: 'AttributeFilter', id: any, attributeName: string, values: Array<{ __typename?: 'AttributeValueFilter', value: string, count: number, unitsOfMeasure?: string | null }> }> } | null } };


export const SearchProductsByNameDocument = gql`
    query SearchProductsByName($skip: Int!, $take: Int!, $productName: String!) {
  searchProductsByName(skip: $skip, take: $take, productName: $productName) {
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

/**
 * __useSearchProductsByNameQuery__
 *
 * To run a query within a React component, call `useSearchProductsByNameQuery` and pass it any options that fit your needs.
 * When your component renders, `useSearchProductsByNameQuery` returns an object from Apollo Client that contains loading, error, and data properties
 * you can use to render your UI.
 *
 * @param baseOptions options that will be passed into the query, supported options are listed on: https://www.apollographql.com/docs/react/api/react-hooks/#options;
 *
 * @example
 * const { data, loading, error } = useSearchProductsByNameQuery({
 *   variables: {
 *      skip: // value for 'skip'
 *      take: // value for 'take'
 *      productName: // value for 'productName'
 *   },
 * });
 */
export function useSearchProductsByNameQuery(baseOptions: Apollo.QueryHookOptions<SearchProductsByNameQuery, SearchProductsByNameQueryVariables> & ({ variables: SearchProductsByNameQueryVariables; skip?: boolean; } | { skip: boolean; }) ) {
        const options = {...defaultOptions, ...baseOptions}
        return Apollo.useQuery<SearchProductsByNameQuery, SearchProductsByNameQueryVariables>(SearchProductsByNameDocument, options);
      }
export function useSearchProductsByNameLazyQuery(baseOptions?: Apollo.LazyQueryHookOptions<SearchProductsByNameQuery, SearchProductsByNameQueryVariables>) {
          const options = {...defaultOptions, ...baseOptions}
          return Apollo.useLazyQuery<SearchProductsByNameQuery, SearchProductsByNameQueryVariables>(SearchProductsByNameDocument, options);
        }
export function useSearchProductsByNameSuspenseQuery(baseOptions?: Apollo.SkipToken | Apollo.SuspenseQueryHookOptions<SearchProductsByNameQuery, SearchProductsByNameQueryVariables>) {
          const options = baseOptions === Apollo.skipToken ? baseOptions : {...defaultOptions, ...baseOptions}
          return Apollo.useSuspenseQuery<SearchProductsByNameQuery, SearchProductsByNameQueryVariables>(SearchProductsByNameDocument, options);
        }
export type SearchProductsByNameQueryHookResult = ReturnType<typeof useSearchProductsByNameQuery>;
export type SearchProductsByNameLazyQueryHookResult = ReturnType<typeof useSearchProductsByNameLazyQuery>;
export type SearchProductsByNameSuspenseQueryHookResult = ReturnType<typeof useSearchProductsByNameSuspenseQuery>;
export type SearchProductsByNameQueryResult = Apollo.QueryResult<SearchProductsByNameQuery, SearchProductsByNameQueryVariables>;
export const SearchProductsWithFiltersDocument = gql`
    query SearchProductsWithFilters($skip: Int!, $take: Int!, $productName: String, $categoryIds: [UUID!], $minPrice: Float, $maxPrice: Float, $inStockOnly: Boolean, $variantAttributesIds: [UUID!], $sortBy: SortBy!, $sortOrder: SortOrder!) {
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

/**
 * __useSearchProductsWithFiltersQuery__
 *
 * To run a query within a React component, call `useSearchProductsWithFiltersQuery` and pass it any options that fit your needs.
 * When your component renders, `useSearchProductsWithFiltersQuery` returns an object from Apollo Client that contains loading, error, and data properties
 * you can use to render your UI.
 *
 * @param baseOptions options that will be passed into the query, supported options are listed on: https://www.apollographql.com/docs/react/api/react-hooks/#options;
 *
 * @example
 * const { data, loading, error } = useSearchProductsWithFiltersQuery({
 *   variables: {
 *      skip: // value for 'skip'
 *      take: // value for 'take'
 *      productName: // value for 'productName'
 *      categoryIds: // value for 'categoryIds'
 *      minPrice: // value for 'minPrice'
 *      maxPrice: // value for 'maxPrice'
 *      inStockOnly: // value for 'inStockOnly'
 *      variantAttributesIds: // value for 'variantAttributesIds'
 *      sortBy: // value for 'sortBy'
 *      sortOrder: // value for 'sortOrder'
 *   },
 * });
 */
export function useSearchProductsWithFiltersQuery(baseOptions: Apollo.QueryHookOptions<SearchProductsWithFiltersQuery, SearchProductsWithFiltersQueryVariables> & ({ variables: SearchProductsWithFiltersQueryVariables; skip?: boolean; } | { skip: boolean; }) ) {
        const options = {...defaultOptions, ...baseOptions}
        return Apollo.useQuery<SearchProductsWithFiltersQuery, SearchProductsWithFiltersQueryVariables>(SearchProductsWithFiltersDocument, options);
      }
export function useSearchProductsWithFiltersLazyQuery(baseOptions?: Apollo.LazyQueryHookOptions<SearchProductsWithFiltersQuery, SearchProductsWithFiltersQueryVariables>) {
          const options = {...defaultOptions, ...baseOptions}
          return Apollo.useLazyQuery<SearchProductsWithFiltersQuery, SearchProductsWithFiltersQueryVariables>(SearchProductsWithFiltersDocument, options);
        }
export function useSearchProductsWithFiltersSuspenseQuery(baseOptions?: Apollo.SkipToken | Apollo.SuspenseQueryHookOptions<SearchProductsWithFiltersQuery, SearchProductsWithFiltersQueryVariables>) {
          const options = baseOptions === Apollo.skipToken ? baseOptions : {...defaultOptions, ...baseOptions}
          return Apollo.useSuspenseQuery<SearchProductsWithFiltersQuery, SearchProductsWithFiltersQueryVariables>(SearchProductsWithFiltersDocument, options);
        }
export type SearchProductsWithFiltersQueryHookResult = ReturnType<typeof useSearchProductsWithFiltersQuery>;
export type SearchProductsWithFiltersLazyQueryHookResult = ReturnType<typeof useSearchProductsWithFiltersLazyQuery>;
export type SearchProductsWithFiltersSuspenseQueryHookResult = ReturnType<typeof useSearchProductsWithFiltersSuspenseQuery>;
export type SearchProductsWithFiltersQueryResult = Apollo.QueryResult<SearchProductsWithFiltersQuery, SearchProductsWithFiltersQueryVariables>;