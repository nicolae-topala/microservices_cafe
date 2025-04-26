'use client';

import { useEffect } from 'react';
import { useForm } from 'react-hook-form';

import {
    useCreateCategoryMutation,
    useGetCategoriesLazyQuery,
} from '@/graphql/categories/queries.generated';

type FormValues = {
    name: string;
};

const DataPage = () => {
    const [getCategories, { data, loading, error }] =
        useGetCategoriesLazyQuery();

    const [createCategoryMutation] = useCreateCategoryMutation();

    useEffect(() => {
        getCategories();
    }, []);

    const { reset, register, handleSubmit } = useForm<FormValues>({
        defaultValues: { name: '' },
    });

    const onSubmit = async (values: FormValues) => {
        try {
            await createCategoryMutation({
                variables: {
                    name: values.name,
                },
            });

            await getCategories();
            reset();
        } catch (err) {
            console.error(err);
        }
    };

    if (loading) {
        return <p>Loading...</p>;
    }

    if (error?.message) {
        return <p>{error.message}</p>;
    }

    return (
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

            <h3>Create a New Category</h3>
            <div>
                <form onSubmit={handleSubmit(onSubmit)}>
                    <label htmlFor="name">Category Name</label>
                    <input id="name" {...register('name')} />
                    <input type="submit" value="Submit" />
                </form>
            </div>
        </div>
    );
};

export default DataPage;
