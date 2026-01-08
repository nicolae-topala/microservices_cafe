'use client';

import { zodResolver } from '@hookform/resolvers/zod';
import {
    parseAsArrayOf,
    parseAsBoolean,
    parseAsInteger,
    parseAsString,
    parseAsStringEnum,
    useQueryStates,
} from 'nuqs';
import {
    createContext,
    useContext,
    ReactNode,
    useEffect,
    useCallback,
} from 'react';
import { useForm, UseFormReturn } from 'react-hook-form';

import { SortBy, SortOrder } from '@/generated/types';
import {
    SearchProductsByNameQuery,
    SearchProductsWithFiltersQuery,
    useSearchProductsByNameLazyQuery,
    useSearchProductsWithFiltersLazyQuery,
} from '@/graphql/products/queries.generated';

import { shopFormSchema, ShopFormValues } from '../../types/shop';

interface ShopContextValue {
    form: UseFormReturn<ShopFormValues>;
    searchResults?: SearchProductsByNameQuery;
    filterSearchResults?: SearchProductsWithFiltersQuery;
    searchLoading: boolean;
    filterSearchLoading: boolean;
    pageSize: number;
    handleSubmit: () => void;
    handleSearchByNameSubmit: () => void;
    handleSearchWithFiltersSubmit: () => void;
    handleWishlistToggle: (productId: number) => void;
    handleAddToCart: (productId: number) => void;
}

interface ShopProviderProps {
    children: ReactNode;
}

export const ShopProvider = ({ children }: ShopProviderProps) => {
    const pageSize = 9;

    const [searchProductsByName, { data: searchData, loading: searchLoading }] =
        useSearchProductsByNameLazyQuery();

    const [
        searchProductsWithFilters,
        { data: filterSearchData, loading: filterSearchLoading },
    ] = useSearchProductsWithFiltersLazyQuery();

    const [queryParams, setQueryParams] = useQueryStates({
        categories: parseAsArrayOf(parseAsString),
        minPrice: parseAsInteger.withDefault(0),
        maxPrice: parseAsInteger,
        sortBy: parseAsStringEnum<SortBy>(Object.values(SortBy)).withDefault(
            SortBy.Popularity
        ),
        sortOrder: parseAsStringEnum<SortOrder>(
            Object.values(SortOrder)
        ).withDefault(SortOrder.Desc),
        page: parseAsInteger.withDefault(1),
        search: parseAsString,
        inStockOnly: parseAsBoolean.withDefault(false),
        variantAttributes: parseAsArrayOf(parseAsString),
    });

    const formValues: ShopFormValues = {
        categories: queryParams.categories ?? undefined,
        priceRange: {
            min: queryParams.minPrice,
            max: queryParams.maxPrice ?? undefined,
        },
        maxAvailablePrice: undefined,
        sortBy: queryParams.sortBy,
        sortOrder: queryParams.sortOrder,
        page: queryParams.page,
        search: queryParams.search ?? undefined,
        inStockOnly: queryParams.inStockOnly,
        variantAttributes: queryParams.variantAttributes ?? undefined,
    };

    const form = useForm<ShopFormValues>({
        resolver: zodResolver(shopFormSchema),
        defaultValues: formValues,
    });

    const updateUrlFromForm = useCallback(
        (data: ShopFormValues) => {
            setQueryParams({
                categories:
                    data.categories && data.categories.length > 0
                        ? data.categories
                        : null,
                minPrice: data.priceRange.min ?? null,
                maxPrice: data.priceRange.max ?? null,
                sortBy: data.sortBy,
                page: data.page,
                search: data.search ?? null,
                inStockOnly: data.inStockOnly,
                variantAttributes:
                    data.variantAttributes && data.variantAttributes.length > 0
                        ? data.variantAttributes
                        : null,
            });
        },
        [setQueryParams]
    );

    const handleSearchByNameSubmit = useCallback(async () => {
        const searchTerm = form.getValues().search;
        const pageNumber = form.getValues().page;

        if (searchTerm === undefined || searchTerm!.trim() === '') {
            return;
        }

        try {
            await searchProductsByName({
                variables: {
                    skip: (pageNumber - 1) * pageSize,
                    take: pageSize,
                    productName: searchTerm!,
                },
            });
        } catch (error) {
            console.error('Search error:', error);
        }
    }, [form, searchProductsByName, pageSize]);

    const handleSearchWithFiltersSubmit = useCallback(async () => {
        const values = form.getValues();

        try {
            const result = await searchProductsWithFilters({
                variables: {
                    skip: (values.page - 1) * pageSize,
                    take: pageSize,
                    productName: values.search,
                    categoryIds: values.categories,
                    minPrice: values.priceRange.min,
                    maxPrice: values.priceRange.max,
                    inStockOnly: values.inStockOnly,
                    variantAttributesIds: values.variantAttributes,
                    sortBy: values.sortBy,
                    sortOrder: values.sortOrder,
                },
            });

            const maxPriceFromResults =
                result.data?.searchProductsWithFilters.filters?.priceRange
                    .maxPrice;
            if (
                maxPriceFromResults !== undefined &&
                maxPriceFromResults !== null
            ) {
                form.setValue('maxAvailablePrice', maxPriceFromResults);
            }
        } catch (error) {
            console.error('Filter search error:', error);
        }
    }, [form, searchProductsWithFilters, pageSize]);

    const handleSubmit = form.handleSubmit(() => {
        void handleSearchWithFiltersSubmit();
    });

    const handleWishlistToggle = (productId: number) => {
        // TODO: Implement wishlist toggle logic
        void productId;
    };

    const handleAddToCart = (productId: number) => {
        // TODO: Implement add to cart logic
        void productId;
    };

    useEffect(() => {
        // Initial data load
        handleSearchWithFiltersSubmit();

        // Watch form changes
        const subscription = form.watch((_, { name }) => {
            if (name === 'maxAvailablePrice') {
                return;
            }

            switch (name) {
                case undefined:
                    break;
                case 'search':
                    handleSearchByNameSubmit();
                    break;
                default:
                    updateUrlFromForm(form.getValues());
                    handleSearchWithFiltersSubmit();
                    break;
            }
        });

        return () => {
            return subscription.unsubscribe();
        };
    }, []);

    const value: ShopContextValue = {
        form,
        searchResults: searchData,
        filterSearchResults: filterSearchData,
        searchLoading,
        filterSearchLoading,
        pageSize,
        handleSubmit,
        handleSearchByNameSubmit,
        handleSearchWithFiltersSubmit,
        handleWishlistToggle,
        handleAddToCart,
    };

    return (
        <ShopContext.Provider value={value}>{children}</ShopContext.Provider>
    );
};

const ShopContext = createContext<ShopContextValue | undefined>(undefined);
export const useShopContext = () => {
    const context = useContext(ShopContext);
    if (context === undefined) {
        throw new Error('useShopContext must be used within a ShopProvider');
    }
    return context;
};
