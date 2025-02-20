import { gql } from '@apollo/client';

export const GET_USERS = gql`
    query GetCategories {
        categories(first: 5) {
            nodes {
                id
                name
            }
        }
    }
`;

export const CREATE_CATEGORY = gql`
    mutation CreateCategory($name: String!) {
        createCategory(input: { category: { name: $name } }) {
            category {
                id
                name
            }
        }
    }
`;
