'use client';

import { FormControl, FormField, FormItem } from '@/components/common/ui/form';
import {
    Select,
    SelectContent,
    SelectItem,
    SelectTrigger,
    SelectValue,
} from '@/components/common/ui/select';
import { useShopContext } from '@/contexts/shop/use-shop-context';
import { SORT_OPTIONS } from '@/types/shop';

const SortSelector = () => {
    const { form } = useShopContext();

    return (
        <FormField
            control={form.control}
            name="sortBy"
            render={({ field }) => (
                <FormItem>
                    <FormControl>
                        <Select
                            onValueChange={field.onChange}
                            value={field.value}
                        >
                            <SelectTrigger className="w-40">
                                <SelectValue placeholder="Sort By" />
                            </SelectTrigger>
                            <SelectContent>
                                {SORT_OPTIONS.map((option) => (
                                    <SelectItem
                                        key={option.value}
                                        value={option.value}
                                    >
                                        {option.label}
                                    </SelectItem>
                                ))}
                            </SelectContent>
                        </Select>
                    </FormControl>
                </FormItem>
            )}
        />
    );
};

export default SortSelector;
