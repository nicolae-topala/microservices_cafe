'use client';

import { Form } from '@/components/common/ui/form';
import { useShopContext } from '@/contexts/shop/use-shop-context';

import ShopHeader from './shop-header';
import ShopMain from './shop-main';
import ShopSidebar from './shop-sidebar';

const ShopForm = () => {
    const { form, handleSubmit } = useShopContext();

    return (
        <Form {...form}>
            <form onSubmit={handleSubmit}>
                <ShopHeader />

                <div className="container mx-auto py-6 grid grid-cols-12 gap-8">
                    <ShopSidebar />
                    <ShopMain />
                </div>
            </form>
        </Form>
    );
};

export default ShopForm;
