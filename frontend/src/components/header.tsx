import { auth, signOut } from '@/auth';

function SignOutButton() {
    return (
        <form
            action={async () => {
                'use server';
                await signOut();
            }}
        >
            <button type="submit">Sign out</button>
        </form>
    );
}

export async function Header() {
    const session = await auth();

    return (
        <header>
            <nav>
                <p>Header</p>
                <div>
                    <span>{session?.user?.name}</span>
                    <SignOutButton />
                </div>
            </nav>
        </header>
    );
}
