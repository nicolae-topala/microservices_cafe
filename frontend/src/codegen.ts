import type { CodegenConfig } from '@graphql-codegen/cli';

const config: CodegenConfig = {
    overwrite: true,
    schema: ['./schema.graphql'],
    documents: './src/graphql/**/*.ts',
    generates: {
        'src/generated/types.ts': {
            plugins: ['typescript'],
        },
        'src/generated/': {
            preset: 'near-operation-file',
            presetConfig: {
                extension: '.generated.tsx',
                baseTypesPath: 'types.ts',
            },
            plugins: ['typescript-operations', 'typescript-react-apollo'],
            config: {
                withHooks: true,
            },
        },
    },
};

export default config;
