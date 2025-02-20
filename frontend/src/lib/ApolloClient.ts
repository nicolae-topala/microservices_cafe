import { ApolloClient, HttpLink, InMemoryCache } from '@apollo/client';
import { setContext } from '@apollo/client/link/context';
import { registerApolloClient } from '@apollo/experimental-nextjs-app-support';
import { auth } from '@/auth';

export const { getClient, query, PreloadQuery } = registerApolloClient(() => {
    const httpLink = new HttpLink({
        uri: process.env.NEXT_PUBLIC_BE_GRAPHQL_URL,
    });

    const authLink = setContext(async (_, { headers }) => {
        //Using auth() instead of getServerSession()
        //Because the experimental library bumps to v5 of NextAuth
        //https://github.com/nextauthjs/next-auth/issues/8263
        //https://authjs.dev/getting-started/migrating-to-v5#details
        const session = await auth();
        const token = session?.user.accessToken;

        return {
            headers: {
                ...headers,
                ...(token ? { authorization: `Bearer ${token}` } : {}),
            },
        };
    });

    return new ApolloClient({
        link: authLink.concat(httpLink),
        cache: new InMemoryCache(),
    });
});
