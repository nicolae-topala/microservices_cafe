import { dirname } from 'path';
import { fileURLToPath } from 'url';
import { FlatCompat } from '@eslint/eslintrc';

const __filename = fileURLToPath(import.meta.url);
const __dirname = dirname(__filename);

const compat = new FlatCompat({
  baseDirectory: __dirname
});

const eslintConfig = [
  {
    ignores: [
      'src/generated/',
      'src/graphql/**/*.generated.tsx'
    ],
  },
  ...compat.extends('next/core-web-vitals', 'next/typescript'),
  ...compat.extends('plugin:prettier/recommended'),
  {
    rules: {
      'prettier/prettier': 'error',
      eqeqeq: ['error', 'always'],                                // Require use of === and !==
      'no-console': ['warn', { allow: ['warn', 'error'] }],       // Warn on console usage except `console.warn` and `console.error`
      'no-var': ['error'],                                        // Disallow the use of var, prefer const/let
      'prefer-const': ['error'],                                  // Suggest using const
      curly: ['error', 'all'],                                    // Enforce consistent brace style for blocks
    }
  }
];

export default eslintConfig;