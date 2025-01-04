import NextAuth from 'next-auth';
// import { authConfig } from './auth.config';

export const { handlers, auth, signIn, signOut } = NextAuth({
    // ...authConfig,
    providers: [
        {
            id: 'authServer',
            name: 'Auth Server',
            type: 'oidc',
            issuer: process.env.AUTH_OIDC_ISSUER,
            clientId: process.env.AUTH_OIDC_ID,
            clientSecret: process.env.AUTH_OIDC_SECRET,
            profile(profile) {
                console.log('User logged in', { userId: profile.sub });
                return {
                    id: profile.sub,
                    username: profile.sub,
                    name: profile.name,
                    email: profile.email,
                };
            },
        },
    ],
    pages: {
        error: '/sign-in',
        signIn: '/sign-in',
        signOut: '/sign-in',
    },
    callbacks: {
        jwt({ token, user }) {
            if (user) {
                token.username = user.username;
            }
            return token;
        },
        session({ session, token }) {
            session.user.username = token.username;
            return session;
        },
    },
});
