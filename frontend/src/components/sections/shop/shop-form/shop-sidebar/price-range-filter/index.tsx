'use client';

import { useCallback, useState } from 'react';
import { useDebouncedCallback } from 'use-debounce';

import TTypography from '@/components/common/i18n/TTypography';
import { FormControl, FormField, FormItem } from '@/components/common/ui/form';
import { Input } from '@/components/common/ui/input';
import { Slider } from '@/components/common/ui/slider';
import { useShopContext } from '@/contexts/shop/use-shop-context';

const PriceRangeFilter = () => {
    const { form } = useShopContext();
    const maxAvailablePrice = form.watch('maxAvailablePrice') ?? 100;
    const debounceWait = 300;

    const [localPriceRange, setLocalPriceRange] = useState(
        form.getValues('priceRange')
    );

    const debouncedSetValue = useDebouncedCallback(
        (value: { min: number; max: number | undefined }) => {
            form.setValue('priceRange', value, {
                shouldValidate: true,
            });
        },
        debounceWait
    );

    const handlePriceChange = useCallback(
        (newValue: { min: number; max: number | undefined }) => {
            setLocalPriceRange(newValue);
            debouncedSetValue(newValue);
        },
        [debouncedSetValue]
    );

    return (
        <div className="space-y-3">
            <TTypography
                component="h3"
                namespace="filters"
                messageKey="Price"
            />
            <div className="space-y-3">
                <FormField
                    control={form.control}
                    name="priceRange"
                    render={() => (
                        <FormItem>
                            <FormControl>
                                <Slider
                                    className="w-full"
                                    max={maxAvailablePrice}
                                    step={1}
                                    value={[
                                        localPriceRange.min,
                                        localPriceRange.max ??
                                            maxAvailablePrice,
                                    ]}
                                    onValueChange={([min, max]) => {
                                        handlePriceChange({ min, max });
                                    }}
                                />
                            </FormControl>
                        </FormItem>
                    )}
                />
                <div className="flex items-center space-x-2 text-sm">
                    <FormField
                        control={form.control}
                        name="priceRange"
                        render={() => (
                            <FormItem>
                                <FormControl>
                                    <Input
                                        className="w-16 p-1 border rounded text-center"
                                        type="number"
                                        min={0}
                                        max={localPriceRange.max}
                                        value={localPriceRange.min}
                                        onChange={(e) =>
                                            handlePriceChange({
                                                min: Number(e.target.value),
                                                max: localPriceRange.max,
                                            })
                                        }
                                    />
                                </FormControl>
                            </FormItem>
                        )}
                    />
                    <span>-</span>
                    <FormField
                        control={form.control}
                        name="priceRange"
                        render={() => (
                            <FormItem>
                                <FormControl>
                                    <Input
                                        className="w-16 p-1 border rounded text-center"
                                        type="number"
                                        value={
                                            localPriceRange.max ??
                                            maxAvailablePrice
                                        }
                                        onChange={(e) =>
                                            handlePriceChange({
                                                ...localPriceRange,
                                                max: Number(e.target.value),
                                            })
                                        }
                                        min={localPriceRange.min}
                                        max={maxAvailablePrice}
                                    />
                                </FormControl>
                            </FormItem>
                        )}
                    />
                </div>
            </div>
        </div>
    );
};

export default PriceRangeFilter;
