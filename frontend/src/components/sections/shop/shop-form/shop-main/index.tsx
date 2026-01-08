import ProductGrid from './product-grid';
import ShopBreadcrumb from './shop-breadcrumb';
import ShopPagination from './shop-pagination';
import SortSelector from './sort-selector';

const ShopMain = () => {
    return (
        <div className="space-y-9 col-span-10">
            <div className="flex items-center justify-between">
                <ShopBreadcrumb />
                <SortSelector />
            </div>

            <ProductGrid />
            <ShopPagination totalPages={10} />
        </div>
    );
};

export default ShopMain;
