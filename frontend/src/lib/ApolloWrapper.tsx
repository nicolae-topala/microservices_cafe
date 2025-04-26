'use client';

import { HttpLink, useApolloClient } from '@apollo/client';
import { setContext } from '@apollo/client/link/context';
import {
    ApolloClient,
    ApolloNextAppProvider,
    InMemoryCache,
} from '@apollo/experimental-nextjs-app-support';
import { useSession } from 'next-auth/react';

//https://github.com/apollographql/apollo-client-nextjs/issues/103#issuecomment-1790941212

const httpLink = new HttpLink({
    uri: process.env.NEXT_PUBLIC_BE_GRAPHQL_URL,
});

const authLink = setContext(async (_, { headers, token }) => {
    return {
        headers: {
            ...headers,
            ...(token ? { authorization: `Bearer ${token}` } : {}),
        },
    };
});

const makeClient = () =>
    new ApolloClient({
        cache: new InMemoryCache(),
        link: authLink.concat(httpLink),
    });

const UpdateAuth = ({ children }: { children: React.ReactNode }) => {
    const { data } = useSession();
    const apolloClient = useApolloClient();

    // just synchronously update the `apolloClient.defaultContext` before any child component can be rendered
    // so the value is available for any query started in a child
    apolloClient.defaultContext.token = data?.user.accessToken ?? undefined;
    return <>{children}</>;
};

const ApolloWrapper = ({ children }: React.PropsWithChildren) => {
    return (
        <ApolloNextAppProvider makeClient={makeClient}>
            <UpdateAuth>{children}</UpdateAuth>
        </ApolloNextAppProvider>
    );
};

export default ApolloWrapper;
