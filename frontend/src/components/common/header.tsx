import { LogOut, Menu, ShoppingBasket, User } from 'lucide-react';
import Image from 'next/image';
import Link from 'next/link';

import { auth, signOut } from '@/auth';

import TButton from './i18n/TButton';
import TTypography from './i18n/TTypography';
import { ModeToggle } from './theme-model-toggle';
import { Button } from './ui/button';
import {
    DropdownMenu,
    DropdownMenuContent,
    DropdownMenuItem,
    DropdownMenuTrigger,
} from './ui/dropdown-menu';
import { Separator } from './ui/separator';

const SignOutButton = () => {
    return (
        <form
            action={async () => {
                'use server';
                await signOut();
            }}
        >
            <Button type="submit" variant="ghost">
                <LogOut />
            </Button>
        </form>
    );
};

export async function Header() {
    const session = await auth();
    const isSession = session !== null;

    return (
        <header>
            <nav className="p-2 container mx-auto">
                <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-2 gap-4 items-center">
                    <div className="flex items-center justify-center md:justify-start gap-3">
                        <Image
                            src="/logo.svg"
                            alt="Logo"
                            width={32}
                            height={32}
                        />
                        <TTypography
                            className="uppercase"
                            component="h3"
                            namespace="app"
                            messageKey="name"
                        />
                    </div>

                    <div className="flex items-center justify-center md:justify-end space-x-1">
                        <Link href="/">
                            <TButton
                                namespace="navbar"
                                messageKey="home"
                                variant="ghost"
                                className="text-sm"
                            />
                        </Link>
                        <TButton
                            namespace="navbar"
                            messageKey="menu"
                            variant="ghost"
                            className="text-sm"
                        />
                        <TButton
                            namespace="navbar"
                            messageKey="locations"
                            variant="ghost"
                            className="text-sm"
                        />
                        <Link href="/shop">
                            <TButton
                                namespace="navbar"
                                messageKey="shop"
                                variant="ghost"
                                className="text-sm"
                            />
                        </Link>
                        <TButton
                            namespace="navbar"
                            messageKey="booknow"
                            variant="outline"
                            className="text-sm"
                        />
                        <div className="ml-5">
                            <ModeToggle />
                            {isSession ? (
                                <SignOutButton />
                            ) : (
                                <Button
                                    variant="ghost"
                                    size="icon"
                                    className="size-8"
                                >
                                    <User className="h-4 w-4" />
                                </Button>
                            )}
                            <Button
                                variant="ghost"
                                size="icon"
                                className="size-8"
                            >
                                <ShoppingBasket className="h-4 w-4" />
                            </Button>
                            <DropdownMenu>
                                <DropdownMenuTrigger asChild>
                                    <Button
                                        variant="ghost"
                                        size="icon"
                                        className="size-8"
                                    >
                                        <Menu className="h-4 w-4" />
                                    </Button>
                                </DropdownMenuTrigger>
                                <DropdownMenuContent>
                                    <DropdownMenuItem>Line 1</DropdownMenuItem>
                                    <DropdownMenuItem>Line 2</DropdownMenuItem>
                                    <DropdownMenuItem>Line 3</DropdownMenuItem>
                                    <DropdownMenuItem>Line 4</DropdownMenuItem>
                                    <DropdownMenuItem>Line 5</DropdownMenuItem>
                                </DropdownMenuContent>
                            </DropdownMenu>
                        </div>
                    </div>
                </div>
            </nav>
            <Separator />
        </header>
    );
}
