export type Maybe<T> = T | null;
export type InputMaybe<T> = Maybe<T>;
export type Exact<T extends { [key: string]: unknown }> = { [K in keyof T]: T[K] };
export type MakeOptional<T, K extends keyof T> = Omit<T, K> & { [SubKey in K]?: Maybe<T[SubKey]> };
export type MakeMaybe<T, K extends keyof T> = Omit<T, K> & { [SubKey in K]: Maybe<T[SubKey]> };
export type MakeEmpty<T extends { [key: string]: unknown }, K extends keyof T> = { [_ in K]?: never };
export type Incremental<T> = T | { [P in keyof T]?: P extends ' $fragmentName' | '__typename' ? T[P] : never };
/** All built-in and custom scalars, mapped to their actual values */
export type Scalars = {
  ID: { input: string; output: string; }
  String: { input: string; output: string; }
  Boolean: { input: boolean; output: boolean; }
  Int: { input: number; output: number; }
  Float: { input: number; output: number; }
  /** The built-in `Decimal` scalar type. */
  Decimal: { input: any; output: any; }
  UUID: { input: any; output: any; }
};

/** Defines when a policy shall be executed. */
export enum ApplyPolicy {
  /** After the resolver was executed. */
  AfterResolver = 'AFTER_RESOLVER',
  /** Before the resolver was executed. */
  BeforeResolver = 'BEFORE_RESOLVER',
  /** The policy is applied in the validation step before the execution. */
  Validation = 'VALIDATION'
}

export type BooleanOperationFilterInput = {
  eq?: InputMaybe<Scalars['Boolean']['input']>;
  neq?: InputMaybe<Scalars['Boolean']['input']>;
};

/** A connection to a list of items. */
export type CategoriesConnection = {
  __typename?: 'CategoriesConnection';
  /** A list of edges. */
  edges?: Maybe<Array<CategoriesEdge>>;
  /** A flattened list of the nodes. */
  nodes?: Maybe<Array<Category>>;
  /** Information to aid in pagination. */
  pageInfo: PageInfo;
};

/** An edge in a connection. */
export type CategoriesEdge = {
  __typename?: 'CategoriesEdge';
  /** A cursor for use in pagination. */
  cursor: Scalars['String']['output'];
  /** The item at the end of the edge. */
  node: Category;
};

export type Category = {
  __typename?: 'Category';
  id: Scalars['ID']['output'];
  name: Scalars['String']['output'];
  parentCategory?: Maybe<Category>;
  products: Array<Maybe<Product>>;
  subCategories?: Maybe<Array<Maybe<Category>>>;
};

export type CategoryFilterInput = {
  and?: InputMaybe<Array<CategoryFilterInput>>;
  id?: InputMaybe<UuidOperationFilterInput>;
  name?: InputMaybe<StringOperationFilterInput>;
  or?: InputMaybe<Array<CategoryFilterInput>>;
  parentCategory?: InputMaybe<CategoryFilterInput>;
  products?: InputMaybe<ListFilterInputTypeOfProductFilterInput>;
  subCategories?: InputMaybe<ListFilterInputTypeOfCategoryFilterInput>;
};

export type CategorySortInput = {
  id?: InputMaybe<SortEnumType>;
  name?: InputMaybe<SortEnumType>;
  parentCategory?: InputMaybe<CategorySortInput>;
};

export type CreateCategoryDtoInput = {
  name: Scalars['String']['input'];
};

export type CreateCategoryError = ResultError;

export type CreateCategoryInput = {
  category: CreateCategoryDtoInput;
};

export type CreateCategoryPayload = {
  __typename?: 'CreateCategoryPayload';
  category?: Maybe<Category>;
  errors?: Maybe<Array<CreateCategoryError>>;
};

export type CreateProductDtoInput = {
  categoryIds: Array<Scalars['UUID']['input']>;
  currency: CurrencyEnum;
  description: Scalars['String']['input'];
  ingredients: Array<Scalars['String']['input']>;
  name: Scalars['String']['input'];
  price: Scalars['Decimal']['input'];
  type: ProductTypeEnum;
};

export type CreateProductError = ResultError;

export type CreateProductInput = {
  product: CreateProductDtoInput;
};

export type CreateProductPayload = {
  __typename?: 'CreateProductPayload';
  errors?: Maybe<Array<CreateProductError>>;
  product?: Maybe<Product>;
};

export enum CurrencyEnum {
  Eur = 'EUR',
  Usd = 'USD'
}

export type CurrencyEnumOperationFilterInput = {
  eq?: InputMaybe<CurrencyEnum>;
  in?: InputMaybe<Array<CurrencyEnum>>;
  neq?: InputMaybe<CurrencyEnum>;
  nin?: InputMaybe<Array<CurrencyEnum>>;
};

export type DecimalOperationFilterInput = {
  eq?: InputMaybe<Scalars['Decimal']['input']>;
  gt?: InputMaybe<Scalars['Decimal']['input']>;
  gte?: InputMaybe<Scalars['Decimal']['input']>;
  in?: InputMaybe<Array<InputMaybe<Scalars['Decimal']['input']>>>;
  lt?: InputMaybe<Scalars['Decimal']['input']>;
  lte?: InputMaybe<Scalars['Decimal']['input']>;
  neq?: InputMaybe<Scalars['Decimal']['input']>;
  ngt?: InputMaybe<Scalars['Decimal']['input']>;
  ngte?: InputMaybe<Scalars['Decimal']['input']>;
  nin?: InputMaybe<Array<InputMaybe<Scalars['Decimal']['input']>>>;
  nlt?: InputMaybe<Scalars['Decimal']['input']>;
  nlte?: InputMaybe<Scalars['Decimal']['input']>;
};

export type DeleteCategoryError = ResultError;

export type DeleteCategoryInput = {
  categoryId: Scalars['UUID']['input'];
};

export type DeleteCategoryPayload = {
  __typename?: 'DeleteCategoryPayload';
  boolean?: Maybe<Scalars['Boolean']['output']>;
  errors?: Maybe<Array<DeleteCategoryError>>;
};

export type DeleteProductError = ResultError;

export type DeleteProductInput = {
  productId: Scalars['UUID']['input'];
};

export type DeleteProductPayload = {
  __typename?: 'DeleteProductPayload';
  boolean?: Maybe<Scalars['Boolean']['output']>;
  errors?: Maybe<Array<DeleteProductError>>;
};

export type EditCategoryDtoInput = {
  id: Scalars['UUID']['input'];
  name: Scalars['String']['input'];
};

export type EditCategoryError = ResultError;

export type EditCategoryInput = {
  category: EditCategoryDtoInput;
};

export type EditCategoryPayload = {
  __typename?: 'EditCategoryPayload';
  category?: Maybe<Category>;
  errors?: Maybe<Array<EditCategoryError>>;
};

export type EditProductDtoInput = {
  categoryId?: InputMaybe<Scalars['UUID']['input']>;
  currency?: InputMaybe<CurrencyEnum>;
  description?: InputMaybe<Scalars['String']['input']>;
  id: Scalars['UUID']['input'];
  ingredients?: InputMaybe<Array<Scalars['String']['input']>>;
  isInStock?: InputMaybe<Scalars['Boolean']['input']>;
  isVisible?: InputMaybe<Scalars['Boolean']['input']>;
  name?: InputMaybe<Scalars['String']['input']>;
  price?: InputMaybe<Scalars['Decimal']['input']>;
  type?: InputMaybe<ProductTypeEnum>;
};

export type EditProductError = ResultError;

export type EditProductInput = {
  product: EditProductDtoInput;
};

export type EditProductPayload = {
  __typename?: 'EditProductPayload';
  errors?: Maybe<Array<EditProductError>>;
  product?: Maybe<Product>;
};

export type Error = {
  message: Scalars['String']['output'];
};

export type ListFilterInputTypeOfCategoryFilterInput = {
  all?: InputMaybe<CategoryFilterInput>;
  any?: InputMaybe<Scalars['Boolean']['input']>;
  none?: InputMaybe<CategoryFilterInput>;
  some?: InputMaybe<CategoryFilterInput>;
};

export type ListFilterInputTypeOfProductFilterInput = {
  all?: InputMaybe<ProductFilterInput>;
  any?: InputMaybe<Scalars['Boolean']['input']>;
  none?: InputMaybe<ProductFilterInput>;
  some?: InputMaybe<ProductFilterInput>;
};

export type ListStringOperationFilterInput = {
  all?: InputMaybe<StringOperationFilterInput>;
  any?: InputMaybe<Scalars['Boolean']['input']>;
  none?: InputMaybe<StringOperationFilterInput>;
  some?: InputMaybe<StringOperationFilterInput>;
};

export type Mutation = {
  __typename?: 'Mutation';
  createCategory: CreateCategoryPayload;
  createProduct: CreateProductPayload;
  deleteCategory: DeleteCategoryPayload;
  deleteProduct: DeleteProductPayload;
  editCategory: EditCategoryPayload;
  editProduct: EditProductPayload;
};


export type MutationCreateCategoryArgs = {
  input: CreateCategoryInput;
};


export type MutationCreateProductArgs = {
  input: CreateProductInput;
};


export type MutationDeleteCategoryArgs = {
  input: DeleteCategoryInput;
};


export type MutationDeleteProductArgs = {
  input: DeleteProductInput;
};


export type MutationEditCategoryArgs = {
  input: EditCategoryInput;
};


export type MutationEditProductArgs = {
  input: EditProductInput;
};

/** Information about pagination in a connection. */
export type PageInfo = {
  __typename?: 'PageInfo';
  /** When paginating forwards, the cursor to continue. */
  endCursor?: Maybe<Scalars['String']['output']>;
  /** Indicates whether more edges exist following the set defined by the clients arguments. */
  hasNextPage: Scalars['Boolean']['output'];
  /** Indicates whether more edges exist prior the set defined by the clients arguments. */
  hasPreviousPage: Scalars['Boolean']['output'];
  /** When paginating backwards, the cursor to continue. */
  startCursor?: Maybe<Scalars['String']['output']>;
};

export type Price = {
  __typename?: 'Price';
  amount: Scalars['Decimal']['output'];
  currency: CurrencyEnum;
};

export type PriceFilterInput = {
  amount?: InputMaybe<DecimalOperationFilterInput>;
  and?: InputMaybe<Array<PriceFilterInput>>;
  currency?: InputMaybe<CurrencyEnumOperationFilterInput>;
  or?: InputMaybe<Array<PriceFilterInput>>;
};

export type PriceSortInput = {
  amount?: InputMaybe<SortEnumType>;
  currency?: InputMaybe<SortEnumType>;
};

export type Product = {
  __typename?: 'Product';
  categories: Array<Maybe<Category>>;
  description: Scalars['String']['output'];
  id: Scalars['ID']['output'];
  ingredients: Array<Maybe<Scalars['String']['output']>>;
  isInStock: Scalars['Boolean']['output'];
  isVisible: Scalars['Boolean']['output'];
  name: Scalars['String']['output'];
  price: Price;
  type: ProductTypeEnum;
};

export type ProductFilterInput = {
  and?: InputMaybe<Array<ProductFilterInput>>;
  categories?: InputMaybe<ListFilterInputTypeOfCategoryFilterInput>;
  description?: InputMaybe<StringOperationFilterInput>;
  id?: InputMaybe<UuidOperationFilterInput>;
  ingredients?: InputMaybe<ListStringOperationFilterInput>;
  isInStock?: InputMaybe<BooleanOperationFilterInput>;
  isVisible?: InputMaybe<BooleanOperationFilterInput>;
  name?: InputMaybe<StringOperationFilterInput>;
  or?: InputMaybe<Array<ProductFilterInput>>;
  price?: InputMaybe<PriceFilterInput>;
  type?: InputMaybe<ProductTypeEnumOperationFilterInput>;
};

export type ProductSortInput = {
  description?: InputMaybe<SortEnumType>;
  id?: InputMaybe<SortEnumType>;
  isInStock?: InputMaybe<SortEnumType>;
  isVisible?: InputMaybe<SortEnumType>;
  name?: InputMaybe<SortEnumType>;
  price?: InputMaybe<PriceSortInput>;
  type?: InputMaybe<SortEnumType>;
};

export enum ProductTypeEnum {
  Offline = 'OFFLINE',
  Online = 'ONLINE'
}

export type ProductTypeEnumOperationFilterInput = {
  eq?: InputMaybe<ProductTypeEnum>;
  in?: InputMaybe<Array<ProductTypeEnum>>;
  neq?: InputMaybe<ProductTypeEnum>;
  nin?: InputMaybe<Array<ProductTypeEnum>>;
};

/** A connection to a list of items. */
export type ProductsConnection = {
  __typename?: 'ProductsConnection';
  /** A list of edges. */
  edges?: Maybe<Array<ProductsEdge>>;
  /** A flattened list of the nodes. */
  nodes?: Maybe<Array<Product>>;
  /** Information to aid in pagination. */
  pageInfo: PageInfo;
};

/** An edge in a connection. */
export type ProductsEdge = {
  __typename?: 'ProductsEdge';
  /** A cursor for use in pagination. */
  cursor: Scalars['String']['output'];
  /** The item at the end of the edge. */
  node: Product;
};

export type Query = {
  __typename?: 'Query';
  categories?: Maybe<CategoriesConnection>;
  products?: Maybe<ProductsConnection>;
  userInfo: UserInfoDto;
};


export type QueryCategoriesArgs = {
  after?: InputMaybe<Scalars['String']['input']>;
  before?: InputMaybe<Scalars['String']['input']>;
  first?: InputMaybe<Scalars['Int']['input']>;
  last?: InputMaybe<Scalars['Int']['input']>;
  order?: InputMaybe<Array<CategorySortInput>>;
  where?: InputMaybe<CategoryFilterInput>;
};


export type QueryProductsArgs = {
  after?: InputMaybe<Scalars['String']['input']>;
  before?: InputMaybe<Scalars['String']['input']>;
  first?: InputMaybe<Scalars['Int']['input']>;
  last?: InputMaybe<Scalars['Int']['input']>;
  order?: InputMaybe<Array<ProductSortInput>>;
  where?: InputMaybe<ProductFilterInput>;
};

export type ResultError = Error & {
  __typename?: 'ResultError';
  code: Scalars['String']['output'];
  message: Scalars['String']['output'];
};

export enum SortEnumType {
  Asc = 'ASC',
  Desc = 'DESC'
}

export type StringOperationFilterInput = {
  and?: InputMaybe<Array<StringOperationFilterInput>>;
  contains?: InputMaybe<Scalars['String']['input']>;
  endsWith?: InputMaybe<Scalars['String']['input']>;
  eq?: InputMaybe<Scalars['String']['input']>;
  in?: InputMaybe<Array<InputMaybe<Scalars['String']['input']>>>;
  ncontains?: InputMaybe<Scalars['String']['input']>;
  nendsWith?: InputMaybe<Scalars['String']['input']>;
  neq?: InputMaybe<Scalars['String']['input']>;
  nin?: InputMaybe<Array<InputMaybe<Scalars['String']['input']>>>;
  nstartsWith?: InputMaybe<Scalars['String']['input']>;
  or?: InputMaybe<Array<StringOperationFilterInput>>;
  startsWith?: InputMaybe<Scalars['String']['input']>;
};

export type UserInfoDto = {
  __typename?: 'UserInfoDto';
  avatarUrl?: Maybe<Scalars['String']['output']>;
  email?: Maybe<Scalars['String']['output']>;
  sub?: Maybe<Scalars['String']['output']>;
  userName?: Maybe<Scalars['String']['output']>;
};

export type UuidOperationFilterInput = {
  eq?: InputMaybe<Scalars['UUID']['input']>;
  gt?: InputMaybe<Scalars['UUID']['input']>;
  gte?: InputMaybe<Scalars['UUID']['input']>;
  in?: InputMaybe<Array<InputMaybe<Scalars['UUID']['input']>>>;
  lt?: InputMaybe<Scalars['UUID']['input']>;
  lte?: InputMaybe<Scalars['UUID']['input']>;
  neq?: InputMaybe<Scalars['UUID']['input']>;
  ngt?: InputMaybe<Scalars['UUID']['input']>;
  ngte?: InputMaybe<Scalars['UUID']['input']>;
  nin?: InputMaybe<Array<InputMaybe<Scalars['UUID']['input']>>>;
  nlt?: InputMaybe<Scalars['UUID']['input']>;
  nlte?: InputMaybe<Scalars['UUID']['input']>;
};
