import { z } from 'zod';

import { SortBy, SortOrder } from '@/generated/types';

export const shopFormSchema = z.object({
    categories: z.array(z.guid()).optional(),
    priceRange: z.object({
        min: z.number().min(0),
        max: z.number().min(0).optional(),
    }),
    maxAvailablePrice: z.number().min(0).optional(),
    sortBy: z.enum(SortBy).default(SortBy.Popularity),
    sortOrder: z.enum(SortOrder).default(SortOrder.Desc),
    page: z.number().min(1),
    search: z.string().optional(),
    inStockOnly: z.boolean().optional(),
    variantAttributes: z.array(z.guid()).optional(),
});

export type ShopFormValues = z.infer<typeof shopFormSchema>;

export interface SortOption {
    value: string;
    label: string;
}

export interface CategoryOption {
    value: string;
    label: string;
}

export const CATEGORY_OPTIONS: CategoryOption[] = [
    { value: 'coffee', label: 'Coffee' },
    { value: 'clothing', label: 'Clothing' },
    { value: 'pottery', label: 'Pottery' },
    { value: 'cat-products', label: 'Cat Products' },
    { value: 'gift-cards', label: 'Gift Cards' },
];

export const SORT_OPTIONS: SortOption[] = [
    { value: 'name', label: 'Name' },
    { value: 'price-low', label: 'Price: Low to High' },
    { value: 'price-high', label: 'Price: High to Low' },
];

export const URL_PARAMS = {
    CATEGORY_TAB: 'categoryTab',
    CATEGORIES: 'categories',
    MIN_PRICE: 'minPrice',
    MAX_PRICE: 'maxPrice',
    SORT_BY: 'sortBy',
    PAGE: 'page',
    SEARCH: 'search',
};
