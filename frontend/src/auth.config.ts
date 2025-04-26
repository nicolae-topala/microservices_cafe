import { NextAuthConfig } from 'next-auth';

export default {
    providers: [
        {
            id: 'authServer',
            name: 'Auth Server',
            type: 'oidc',
            issuer: process.env.AUTH_OIDC_ISSUER,
            clientId: process.env.AUTH_OIDC_ID,
            clientSecret: process.env.AUTH_OIDC_SECRET,
            authorization: {
                params: {
                    scope: process.env.AUTH_OIDC_SCOPES,
                },
            },
            profile(profile) {
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
        async jwt({ token, user, account }) {
            if (account?.access_token) {
                token.accessToken = account.access_token;
            }

            if (user) {
                token.username = user.username;
            }
            return token;
        },
        async session({ session, token }) {
            session.user.username = token.username;
            session.user.accessToken = token.accessToken;
            return session;
        },
    },
} satisfies NextAuthConfig;
