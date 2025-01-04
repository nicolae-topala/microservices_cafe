'use client';
import { signIn } from 'next-auth/react';

export default function SignInPage() {
    const handleSignIn = () => signIn('authServer');

    return (
        <div>
            <div>
                <h1>Welcome</h1>
                <div>
                    <button onClick={handleSignIn}>Sign in</button>
                </div>
            </div>
        </div>
    );
}
