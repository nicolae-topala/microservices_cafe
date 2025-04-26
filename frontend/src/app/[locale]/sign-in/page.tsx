'use client';
import { signIn } from 'next-auth/react';

import { Button } from '../components/ui/button';

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
