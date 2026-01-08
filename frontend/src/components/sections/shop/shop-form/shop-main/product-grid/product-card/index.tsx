import { Heart, ShoppingCart } from 'lucide-react';
import Image from 'next/image';

import { Button } from '@/components/common/ui/button';
import {
    Card,
    CardAction,
    CardContent,
    CardDescription,
    CardFooter,
    CardHeader,
    CardTitle,
} from '@/components/common/ui/card';
import { useShopContext } from '@/contexts/shop/use-shop-context';
import { SearchProductsByNameQuery } from '@/graphql/products/queries.generated';

interface ProductCardProps {
    product: SearchProductsByNameQuery['searchProductsByName'][number];
    variant: SearchProductsByNameQuery['searchProductsByName'][number]['variants'][number];
}

const ProductCard = ({ product, variant }: ProductCardProps) => {
    const { handleWishlistToggle, handleAddToCart } = useShopContext();

    const onWishlistClick = () => {
        handleWishlistToggle(product.id);
    };

    const onAddToCart = () => {
        handleAddToCart(product.id);
    };

    return (
        <Card className="grid grid-rows-subgrid row-span-3">
            <CardHeader>
                <CardTitle>{product.name}</CardTitle>
                <CardDescription>{product.description}</CardDescription>
            </CardHeader>
            <CardContent className="px-0">
                <div className="relative h-55 w-full bg-gray-100">
                    <Image
                        src={`/placeholder.jpg`}
                        alt={product.name}
                        fill
                        className="object-cover"
                        sizes="(max-width: 768px) 100vw, (max-width: 1200px) 50vw, 33vw"
                    />
                    <CardAction>
                        <button
                            className="absolute top-3 right-3 p-2 bg-white rounded-full shadow-sm hover:bg-gray-50 transition-colors"
                            onClick={onWishlistClick}
                            aria-label={
                                true //product.isWishlisted
                                    ? 'Remove from wishlist'
                                    : 'Add to wishlist'
                            }
                        >
                            <Heart
                                className={`w-4 h-4 ${
                                    true //product.isWishlisted
                                        ? 'fill-red-500 text-red-500'
                                        : 'text-gray-400'
                                }`}
                            />
                        </button>
                    </CardAction>
                </div>
            </CardContent>
            <CardFooter className="p-4 gap-2 flex items-center justify-between">
                <div className="flex items-center space-x-2">
                    <span>{variant.priceAmount}</span>
                    <span>{variant.priceCurrency}</span>
                </div>
                <Button
                    onClick={onAddToCart}
                    size="sm"
                    aria-label={`Add ${product.name} to cart`}
                >
                    <ShoppingCart className="w-4 h-4" />
                </Button>
            </CardFooter>
        </Card>
    );
};

export default ProductCard;
