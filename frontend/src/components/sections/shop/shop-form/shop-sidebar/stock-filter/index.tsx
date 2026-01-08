'use client';

import { Checkbox } from '@/components/common/ui/checkbox';
import {
    FormControl,
    FormField,
    FormItem,
    FormLabel,
} from '@/components/common/ui/form';
import { Separator } from '@/components/common/ui/separator';
import { useShopContext } from '@/contexts/shop/use-shop-context';

const StockFilter = () => {
    const { form } = useShopContext();

    return (
        <div className="space-y-3">
            <Separator />
            <FormField
                control={form.control}
                name="inStockOnly"
                render={({ field: { value, onChange } }) => (
                    <FormItem className="flex flex-row items-start space-x-3 space-y-0">
                        <FormControl>
                            <Checkbox
                                checked={value ?? false}
                                onCheckedChange={onChange}
                            />
                        </FormControl>
                        <FormLabel className="text-sm font-normal">
                            In Stock Only
                        </FormLabel>
                    </FormItem>
                )}
            />
        </div>
    );
};

export default StockFilter;
