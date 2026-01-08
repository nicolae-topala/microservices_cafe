'use client';

import { useCallback } from 'react';

import { Checkbox } from '@/components/common/ui/checkbox';
import {
    FormControl,
    FormField,
    FormItem,
    FormLabel,
} from '@/components/common/ui/form';
import { Separator } from '@/components/common/ui/separator';
import { useShopContext } from '@/contexts/shop/use-shop-context';

const AttributeFilters = () => {
    const { form, filterSearchResults } = useShopContext();

    const attributes =
        filterSearchResults?.searchProductsWithFilters.filters?.attributes ??
        [];

    const toggleAttribute = useCallback(
        (attributeId: string, checked: boolean) => {
            const currentValues = form.getValues('variantAttributes') ?? [];
            const newValues = checked
                ? [...currentValues, attributeId]
                : currentValues.filter((id) => id !== attributeId);

            form.setValue('variantAttributes', newValues);
        },
        [form]
    );

    if (attributes.length === 0) {
        return null;
    }

    return (
        <div className="space-y-4">
            {attributes.map((attribute) => (
                <div key={attribute.id} className="space-y-3">
                    <Separator />
                    <h3 className="font-semibold text-sm capitalize">
                        {attribute.attributeName.replace(/_/g, ' ')}
                    </h3>
                    <div className="space-y-2">
                        {attribute.values.map((valueFilter) => (
                            <FormField
                                key={`${attribute.id}-${valueFilter.value}`}
                                control={form.control}
                                name="variantAttributes"
                                render={({ field }) => {
                                    const isChecked = (
                                        field.value ?? []
                                    ).includes(attribute.id);

                                    return (
                                        <FormItem className="flex flex-row items-start space-x-3 space-y-0">
                                            <FormControl>
                                                <Checkbox
                                                    checked={isChecked}
                                                    onCheckedChange={(
                                                        checked
                                                    ) =>
                                                        toggleAttribute(
                                                            attribute.id,
                                                            !!checked
                                                        )
                                                    }
                                                />
                                            </FormControl>
                                            <FormLabel className="text-sm font-normal flex-1">
                                                <div className="flex items-center justify-between">
                                                    <span>
                                                        {valueFilter.value}
                                                        {valueFilter.unitsOfMeasure &&
                                                            ` ${valueFilter.unitsOfMeasure}`}
                                                    </span>
                                                    <span className="text-xs text-muted-foreground">
                                                        ({valueFilter.count})
                                                    </span>
                                                </div>
                                            </FormLabel>
                                        </FormItem>
                                    );
                                }}
                            />
                        ))}
                    </div>
                </div>
            ))}
        </div>
    );
};

export default AttributeFilters;
