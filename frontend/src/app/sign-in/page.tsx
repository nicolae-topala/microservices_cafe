'use client';
import { Button } from '@/components/ui/button';
import { signIn } from 'next-auth/react';

export default function SignInPage() {
    const handleSignIn = () => signIn('authServer');

    return (
        <div>
            <div>
                <h1>Welcome</h1>
                <div>
                    <Button onClick={handleSignIn}>Sign in</Button>
                </div>
            </div>
        </div>
    );
}
