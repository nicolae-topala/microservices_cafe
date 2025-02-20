import NextAuth, { NextAuthConfig } from 'next-auth';

export const authConfig: NextAuthConfig = {
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
        jwt({ token, user, account }) {
            if (account?.access_token) {
                token.accessToken = account.access_token;
            }

            if (user) {
                token.username = user.username;
            }
            return token;
        },
        session({ session, token }) {
            session.user.username = token.username;
            session.user.accessToken = token.accessToken;
            return session;
        },
    },
};

export const { handlers, auth, signIn, signOut } = NextAuth(authConfig);
