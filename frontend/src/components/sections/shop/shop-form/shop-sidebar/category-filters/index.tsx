'use client';

import TTypography from '@/components/common/i18n/TTypography';
import { Checkbox } from '@/components/common/ui/checkbox';
import {
    FormControl,
    FormField,
    FormItem,
    FormLabel,
} from '@/components/common/ui/form';
import { useShopContext } from '@/contexts/shop/use-shop-context';
import { CLOTHING_FILTERS } from '@/types/shop';

const CategoryFilters = () => {
    const { form } = useShopContext();
    const categories = CLOTHING_FILTERS;

    return (
        <div className="space-y-3">
            <TTypography
                component="h3"
                namespace="filters"
                messageKey="Category"
            />
            <div className="space-y-2">
                {categories.map((category) => (
                    <FormField
                        key={category.value}
                        control={form.control}
                        name="categories"
                        render={({ field: { value, onChange } }) => (
                            <FormItem className="flex flex-row items-start space-x-3 space-y-0">
                                <FormControl>
                                    <Checkbox
                                        checked={value.includes(category.value)}
                                        onCheckedChange={(checked) => {
                                            if (checked) {
                                                onChange([
                                                    ...value,
                                                    category.value,
                                                ]);
                                            } else {
                                                const newValue = value.filter(
                                                    (cat) =>
                                                        cat !== category.value
                                                );
                                                onChange(newValue);
                                            }
                                        }}
                                    />
                                </FormControl>
                                <FormLabel className="text-sm font-normal">
                                    {category.label}
                                </FormLabel>
                            </FormItem>
                        )}
                    />
                ))}
            </div>
        </div>
    );
};

export default CategoryFilters;
