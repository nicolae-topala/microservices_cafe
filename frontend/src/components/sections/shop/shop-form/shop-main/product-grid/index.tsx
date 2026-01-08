import { Skeleton } from '@/components/common/ui/skeleton';
import { useShopContext } from '@/contexts/shop/use-shop-context';

import ProductCard from './product-card';

const ProductGrid = () => {
    const { filterSearchResults, filterSearchLoading, pageSize } =
        useShopContext();

    // Only use filter search results for the main grid
    const isLoading = filterSearchLoading;
    const products = filterSearchResults?.searchProductsWithFilters.products;
    const totalCount =
        filterSearchResults?.searchProductsWithFilters.totalCount ?? 0;

    console.log(filterSearchResults);

    return (
        <div className="space-y-4">
            {totalCount > 0 && !isLoading && (
                <div className="text-sm text-muted-foreground">
                    Found {totalCount} product{totalCount !== 1 ? 's' : ''}
                </div>
            )}

            <div className="grid grid-cols-[repeat(auto-fit,minmax(336px,1fr))] gap-y-7 gap-x-10">
                {isLoading &&
                    Array.from({ length: pageSize }).map((_, i) => (
                        <Skeleton key={i} className="h-96 w-full" />
                    ))}

                {!isLoading &&
                    products?.map((product) =>
                        product.variants.map((variant) => (
                            <ProductCard
                                key={variant.id}
                                product={product}
                                variant={variant}
                            />
                        ))
                    )}

                {!isLoading && products && products.length === 0 && (
                    <div className="col-span-full text-center py-12">
                        <p className="text-lg text-muted-foreground">
                            No products found matching your criteria
                        </p>
                    </div>
                )}

                {!isLoading && !filterSearchResults && (
                    <div className="col-span-full text-center py-12">
                        <p className="text-lg text-muted-foreground">
                            Use filters and search to see products
                        </p>
                    </div>
                )}
            </div>
        </div>
    );
};

export default ProductGrid;
