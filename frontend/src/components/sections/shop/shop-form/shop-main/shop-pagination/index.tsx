'use client';

import { FormControl, FormField, FormItem } from '@/components/common/ui/form';
import {
    Pagination,
    PaginationContent,
    PaginationEllipsis,
    PaginationItem,
    PaginationLink,
    PaginationNext,
    PaginationPrevious,
} from '@/components/common/ui/pagination';

import { useShopContext } from '../../../../../../contexts/shop/use-shop-context';

interface ShopPaginationProps {
    totalPages: number;
}

const ShopPagination = ({ totalPages }: ShopPaginationProps) => {
    const { form } = useShopContext();
    const maxPagesToShow = 3;
    const firstPage = 1;
    const lastPage = totalPages;
    const currentPage = form.getValues('page');

    if (totalPages < 1) {
        return null;
    }

    return (
        <FormField
            control={form.control}
            name="page"
            render={({ field: { value, onChange } }) => (
                <FormItem>
                    <FormControl>
                        <Pagination>
                            <PaginationContent>
                                <PaginationItem>
                                    <PaginationPrevious
                                        href="#"
                                        onClick={(e) => {
                                            e.preventDefault();
                                            onChange(value--);
                                        }}
                                        className={
                                            value === firstPage
                                                ? 'pointer-events-none opacity-50'
                                                : ''
                                        }
                                    />
                                </PaginationItem>

                                {currentPage > maxPagesToShow && (
                                    <>
                                        <PaginationItem>
                                            <PaginationLink
                                                href="#"
                                                onClick={(e) => {
                                                    e.preventDefault();
                                                    onChange(firstPage);
                                                }}
                                            >
                                                {firstPage}
                                            </PaginationLink>
                                        </PaginationItem>
                                        <PaginationItem>
                                            <PaginationEllipsis />
                                        </PaginationItem>
                                    </>
                                )}

                                {currentPage === maxPagesToShow && (
                                    <PaginationItem>
                                        <PaginationLink
                                            href="#"
                                            onClick={(e) => {
                                                e.preventDefault();
                                                onChange(firstPage);
                                            }}
                                        >
                                            {firstPage}
                                        </PaginationLink>
                                    </PaginationItem>
                                )}

                                {Array.from(
                                    { length: maxPagesToShow },
                                    (_, i) => {
                                        const splitMaxPages = Math.floor(
                                            maxPagesToShow / 2
                                        );

                                        const pageIndex =
                                            currentPage - splitMaxPages + i;

                                        if (
                                            pageIndex <= 0 ||
                                            pageIndex > totalPages
                                        ) {
                                            return null;
                                        }

                                        return (
                                            <PaginationItem key={pageIndex}>
                                                <PaginationLink
                                                    href="#"
                                                    isActive={
                                                        value === pageIndex
                                                    }
                                                    onClick={(e) => {
                                                        e.preventDefault();
                                                        onChange(pageIndex);
                                                    }}
                                                >
                                                    {pageIndex}
                                                </PaginationLink>
                                            </PaginationItem>
                                        );
                                    }
                                )}

                                {currentPage <= lastPage - maxPagesToShow && (
                                    <>
                                        <PaginationItem>
                                            <PaginationEllipsis />
                                        </PaginationItem>
                                        <PaginationItem>
                                            <PaginationLink
                                                href="#"
                                                onClick={(e) => {
                                                    e.preventDefault();
                                                    onChange(lastPage);
                                                }}
                                            >
                                                {lastPage}
                                            </PaginationLink>
                                        </PaginationItem>
                                    </>
                                )}

                                <PaginationItem>
                                    <PaginationNext
                                        href="#"
                                        onClick={(e) => {
                                            e.preventDefault();
                                            onChange(value++);
                                        }}
                                        className={
                                            value === totalPages
                                                ? 'pointer-events-none opacity-50'
                                                : ''
                                        }
                                    />
                                </PaginationItem>
                            </PaginationContent>
                        </Pagination>
                    </FormControl>
                </FormItem>
            )}
        />
    );
};

export default ShopPagination;
