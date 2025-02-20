const apolloConfig = {
    client: {
        service: {
            localSchemaFile: 'schema.graphql',
        },
        includes: ['./src/**/*.tsx', './src/**/*.ts'],
        excludes: ['./src/graphql/**/*.generated.tsx'],
    },
};

export default apolloConfig;
