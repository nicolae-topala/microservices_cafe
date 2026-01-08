import AttributeFilters from './attribute-filters';
import PriceRangeFilter from './price-range-filter';
import StockFilter from './stock-filter';

const ShopSidebar = () => {
    return (
        <aside className="space-y-6 col-span-2 hidden md:block">
            <PriceRangeFilter />
            <StockFilter />
            <AttributeFilters />
        </aside>
    );
};

export default ShopSidebar;
