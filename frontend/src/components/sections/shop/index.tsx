import { ShopProvider } from '@/contexts/shop/use-shop-context';

import ShopForm from './shop-form';

const Shop = () => {
    return (
        <ShopProvider>
            <ShopForm />
        </ShopProvider>
    );
};

export default Shop;
