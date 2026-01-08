'use client';

import { FormControl, FormField, FormItem } from '@/components/common/ui/form';
import { Tabs, TabsList, TabsTrigger } from '@/components/common/ui/tabs';
import { useShopContext } from '@/contexts/shop/use-shop-context';
import { CATEGORY_OPTIONS } from '@/types/shop';

const CategoryTabs = () => {
    const { form } = useShopContext();

    return (
        <></>
        // <FormField
        //     control={form.control}
        //     name="categoryTab"
        //     render={({ field: { value, onChange } }) => (
        //         <FormItem>
        //             <FormControl>
        //                 <Tabs value={value} onValueChange={onChange}>
        //                     <TabsList>
        //                         {CATEGORY_OPTIONS.map((category) => (
        //                             <TabsTrigger
        //                                 key={category.value}
        //                                 value={category.value}
        //                             >
        //                                 {category.label}
        //                             </TabsTrigger>
        //                         ))}
        //                     </TabsList>
        //                 </Tabs>
        //             </FormControl>
        //         </FormItem>
        //     )}
        // />
    );
};

export default CategoryTabs;
