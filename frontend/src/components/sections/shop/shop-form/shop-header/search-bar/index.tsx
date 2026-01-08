'use client';

import { Search } from 'lucide-react';
import { useCallback, useState } from 'react';
import { useDebouncedCallback } from 'use-debounce';

import { Badge } from '@/components/common/ui/badge';
import { Button } from '@/components/common/ui/button';
import {
    Command,
    CommandEmpty,
    CommandGroup,
    CommandInput,
    CommandItem,
    CommandList,
} from '@/components/common/ui/command';
import { FormControl, FormField, FormItem } from '@/components/common/ui/form';
import {
    Popover,
    PopoverContent,
    PopoverTrigger,
} from '@/components/common/ui/popover';
import { useShopContext } from '@/contexts/shop/use-shop-context';
import { SearchProductsByNameQuery } from '@/graphql/products/queries.generated';

type SearchProduct = SearchProductsByNameQuery['searchProductsByName'][number];

const SearchBar = () => {
    const {
        form,
        handleSearchByNameSubmit,
        handleSearchWithFiltersSubmit,
        searchResults,
        searchLoading,
    } = useShopContext();
    const [isOpen, setIsOpen] = useState(false);

    const [localSearchTerm, setLocalSearchTerm] = useState(
        form.getValues('search')
    );

    const debouncedSetValue = useDebouncedCallback((value: string) => {
        form.setValue('search', value, {
            shouldValidate: true,
        });
        if (value.trim()) {
            handleSearchByNameSubmit();
            setIsOpen(true);
        } else {
            setIsOpen(false);
        }
    }, 500);

    const handleSearchChange = useCallback(
        (newValue: string) => {
            setLocalSearchTerm(newValue);
            debouncedSetValue(newValue);
        },
        [debouncedSetValue]
    );

    const handleProductSelect = useCallback(
        (product: SearchProduct) => {
            setLocalSearchTerm(product.name);
            form.setValue('search', product.name);
            setIsOpen(false);
            // Trigger full search with filters
            void handleSearchWithFiltersSubmit();
        },
        [form, handleSearchWithFiltersSubmit]
    );

    const handleSearchButtonClick = useCallback(() => {
        setIsOpen(false);
        void handleSearchWithFiltersSubmit();
    }, [handleSearchWithFiltersSubmit]);

    const hasResults =
        searchResults && searchResults.searchProductsByName.length > 0;

    return (
        <div className="relative w-full max-w-md">
            <FormField
                control={form.control}
                name="search"
                render={() => (
                    <FormItem>
                        <FormControl>
                            <Popover
                                open={isOpen}
                                onOpenChange={setIsOpen}
                                modal={false}
                            >
                                <Command shouldFilter={false} loop={false}>
                                    <PopoverTrigger asChild>
                                        <CommandInput
                                            placeholder="Search products..."
                                            className="pr-16"
                                            value={localSearchTerm}
                                            onValueChange={handleSearchChange}
                                            onFocus={() => {
                                                if (
                                                    hasResults ||
                                                    searchLoading
                                                ) {
                                                    setIsOpen(true);
                                                }
                                            }}
                                        />
                                    </PopoverTrigger>
                                    <div className="absolute right-1 top-1/2 -translate-y-1/2 flex gap-1">
                                        <Button
                                            type="button"
                                            variant="ghost"
                                            size="sm"
                                            className="h-7 w-7 p-0"
                                            onClick={handleSearchButtonClick}
                                            title="Search with filters"
                                        >
                                            <Search className="h-3 w-3" />
                                        </Button>
                                    </div>

                                    <PopoverContent
                                        className="p-2"
                                        align="start"
                                        side="bottom"
                                        sideOffset={4}
                                        onOpenAutoFocus={(e) => {
                                            e.preventDefault();
                                        }}
                                        onCloseAutoFocus={(e) => {
                                            e.preventDefault();
                                        }}
                                    >
                                        <CommandList>
                                            {searchLoading ? (
                                                <CommandEmpty>
                                                    Searching...
                                                </CommandEmpty>
                                            ) : hasResults ? (
                                                <CommandGroup>
                                                    {searchResults.searchProductsByName.map(
                                                        (product) => (
                                                            <CommandItem
                                                                key={product.id}
                                                                value={
                                                                    product.name
                                                                }
                                                                onSelect={() =>
                                                                    handleProductSelect(
                                                                        product
                                                                    )
                                                                }
                                                            >
                                                                <SearchResultContent
                                                                    product={
                                                                        product
                                                                    }
                                                                />
                                                            </CommandItem>
                                                        )
                                                    )}
                                                </CommandGroup>
                                            ) : (
                                                <CommandEmpty>
                                                    No products found
                                                </CommandEmpty>
                                            )}
                                        </CommandList>
                                    </PopoverContent>
                                </Command>
                            </Popover>
                        </FormControl>
                    </FormItem>
                )}
            />
        </div>
    );
};

const SearchResultContent = ({ product }: { product: SearchProduct }) => {
    return (
        <div className="flex flex-col gap-1 w-full">
            <span className="font-medium text-sm">{product.name}</span>
            {product.description && (
                <span className="text-xs text-muted-foreground line-clamp-2">
                    {product.description}
                </span>
            )}
            {product.categories && product.categories.length > 0 && (
                <div className="flex flex-wrap gap-1 pt-1">
                    {product.categories.slice(0, 2).map((category) => (
                        <Badge
                            key={category.id}
                            variant="secondary"
                            className="text-xs h-4"
                        >
                            {category.name}
                        </Badge>
                    ))}
                    {product.categories.length > 2 && (
                        <span className="text-xs text-muted-foreground">
                            +{product.categories.length - 2}
                        </span>
                    )}
                </div>
            )}
        </div>
    );
};

export default SearchBar;
