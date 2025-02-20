'use server';

import {
    GetCategoriesDocument,
    GetCategoriesQuery,
} from '@/graphql/categories/queries.generated';
import { getClient } from '@/lib/ApolloClient';
import { Suspense } from 'react';

const DataPage = async () => {
    const { data } = await getClient().query<GetCategoriesQuery>({
        query: GetCategoriesDocument,
    });

    return (
        <Suspense fallback={<div>Loading...</div>}>
            <h1 className="text-3xl font-bold underline">Hello world!</h1>
            <div>
                <h2>Categories</h2>
                {data?.categories?.nodes?.map((category) => (
                    <div key={category.id}>
                        <p>
                            Id: {category.id}
                            <br />
                            Name: {category.name}
                        </p>
                    </div>
                ))}
                <hr />
            </div>
        </Suspense>
    );
};

export default DataPage;
