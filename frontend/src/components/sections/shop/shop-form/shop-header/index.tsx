'use client';

import { Separator } from '@/components/common/ui/separator';

import CategoryTabs from './category-tabs';
import SearchBar from './search-bar';

const ShopHeader = () => {
    return (
        <>
            <div className="flex flex-col items-center p-6 gap-4">
                <SearchBar />
                <CategoryTabs />
            </div>
            <Separator />
        </>
    );
};

export default ShopHeader;
