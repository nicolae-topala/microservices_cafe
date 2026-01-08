'use client';

import Link from 'next/link';

import { useShopContext } from '@/contexts/shop/use-shop-context';
import {
    Breadcrumb,
    BreadcrumbItem,
    BreadcrumbLink,
    BreadcrumbList,
    BreadcrumbPage,
    BreadcrumbSeparator,
} from '@/components/common/ui/breadcrumb';

const ShopBreadcrumb = () => {
    const { form } = useShopContext();
    const category = form.watch('categoryTab') || 'Coffee';

    return (
        <Breadcrumb>
            <BreadcrumbList>
                <BreadcrumbItem>
                    <BreadcrumbLink asChild>
                        <Link href="/">Home</Link>
                    </BreadcrumbLink>
                </BreadcrumbItem>
                <BreadcrumbSeparator />
                <BreadcrumbItem>
                    <BreadcrumbPage>
                        {category.charAt(0).toUpperCase() + category.slice(1)}
                    </BreadcrumbPage>
                </BreadcrumbItem>
            </BreadcrumbList>
        </Breadcrumb>
    );
};

export default ShopBreadcrumb;
