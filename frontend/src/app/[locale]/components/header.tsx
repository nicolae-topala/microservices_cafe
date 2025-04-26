import { auth, signOut } from '@/auth';

import { Button } from './ui/button';

const SignOutButton = () => {
    return (
        <form
            action={async () => {
                'use server';
                await signOut();
            }}
        >
            <Button type="submit">Sign out</Button>
        </form>
    );
};

export async function Header() {
    const session = await auth();
    const isSession = session !== null;

    return (
        <header>
            <nav className="p-2 container mx-auto flex items-center justify-between">
                <div>Logo</div>
                <div>
                    <Button variant="ghost" className="mr-1">
                        Home
                    </Button>
                    <Button variant="ghost" className="mr-1">
                        Shop
                    </Button>
                    <Button variant="ghost" className="mr-1">
                        Menu
                    </Button>
                    <Button variant="ghost" className="mr-1">
                        Locations
                    </Button>
                    <Button variant="ghost" className="mr-1">
                        Book now
                    </Button>
                    <Button className="mr-1">Profile</Button>
                    <Button className="mr-1">Shopping Cart</Button>
                    <Button>BurgerMenu</Button>
                </div>
            </nav>
            <hr />
            {isSession ? <SignOutButton /> : <Button>Sign In</Button>}
        </header>
    );
}
