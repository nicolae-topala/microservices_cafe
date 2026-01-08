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
  /** The `Date` scalar represents an ISO-8601 compliant date type. */
  Date: { input: any; output: any; }
  /** The `DateTime` scalar represents an ISO-8601 compliant date time type. */
  DateTime: { input: any; output: any; }
  /** The `Decimal` scalar type represents a decimal floating-point number. */
  Decimal: { input: any; output: any; }
  /** The `LocalDate` scalar type represents a ISO date string, represented as UTF-8 character sequences YYYY-MM-DD. The scalar follows the specification defined in RFC3339 */
  LocalDate: { input: any; output: any; }
  /** The `Long` scalar type represents non-fractional signed whole 64-bit numeric values. Long can represent values between -(2^63) and 2^63 - 1. */
  Long: { input: any; output: any; }
  UUID: { input: any; output: any; }
};

export type AddProductVariantAttributeDtoInput = {
  attributeDefinitionId: Scalars['UUID']['input'];
  productVariantId: Scalars['UUID']['input'];
  unitsOfMeasureId?: InputMaybe<Scalars['UUID']['input']>;
  value: Scalars['String']['input'];
};

export type AddProductVariantAttributeError = ResultError;

export type AddProductVariantAttributeInput = {
  productVariantAttribute: AddProductVariantAttributeDtoInput;
};

export type AddProductVariantAttributePayload = {
  __typename?: 'AddProductVariantAttributePayload';
  boolean?: Maybe<Scalars['Boolean']['output']>;
  errors?: Maybe<Array<AddProductVariantAttributeError>>;
};

export type AddProductVariantDtoInput = {
  currency: Currency;
  price: Scalars['Decimal']['input'];
  productId: Scalars['UUID']['input'];
};

export type AddProductVariantError = ResultError;

export type AddProductVariantInput = {
  productVariant: AddProductVariantDtoInput;
};

export type AddProductVariantPayload = {
  __typename?: 'AddProductVariantPayload';
  errors?: Maybe<Array<AddProductVariantError>>;
  product?: Maybe<Product>;
};

export type Address = {
  __typename?: 'Address';
  city: Scalars['String']['output'];
  country: Scalars['String']['output'];
  postalCode: Scalars['String']['output'];
  street: Scalars['String']['output'];
};

export type AddressFilterInput = {
  and?: InputMaybe<Array<AddressFilterInput>>;
  city?: InputMaybe<StringOperationFilterInput>;
  country?: InputMaybe<StringOperationFilterInput>;
  or?: InputMaybe<Array<AddressFilterInput>>;
  postalCode?: InputMaybe<StringOperationFilterInput>;
  street?: InputMaybe<StringOperationFilterInput>;
};

export type AddressInput = {
  city: Scalars['String']['input'];
  country: Scalars['String']['input'];
  postalCode: Scalars['String']['input'];
  street: Scalars['String']['input'];
};

export type AddressSortInput = {
  city?: InputMaybe<SortEnumType>;
  country?: InputMaybe<SortEnumType>;
  postalCode?: InputMaybe<SortEnumType>;
  street?: InputMaybe<SortEnumType>;
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

export type AttributeFilter = {
  __typename?: 'AttributeFilter';
  attributeName: Scalars['String']['output'];
  id: Scalars['UUID']['output'];
  values: Array<AttributeValueFilter>;
};

export type AttributeValueFilter = {
  __typename?: 'AttributeValueFilter';
  count: Scalars['Int']['output'];
  unitsOfMeasure?: Maybe<Scalars['String']['output']>;
  value: Scalars['String']['output'];
};

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

export type CategoryDocument = {
  __typename?: 'CategoryDocument';
  id: Scalars['UUID']['output'];
  name: Scalars['String']['output'];
  parentCategoryId?: Maybe<Scalars['UUID']['output']>;
};

export type CategoryFilter = {
  __typename?: 'CategoryFilter';
  count: Scalars['Int']['output'];
  id: Scalars['UUID']['output'];
  name: Scalars['String']['output'];
};

export type CategoryFilterInput = {
  and?: InputMaybe<Array<CategoryFilterInput>>;
  id?: InputMaybe<UuidOperationFilterInput>;
  name?: InputMaybe<StringOperationFilterInput>;
  or?: InputMaybe<Array<CategoryFilterInput>>;
  parentCategory?: InputMaybe<CategoryFilterInput>;
  parentCategoryId?: InputMaybe<UuidOperationFilterInput>;
  products?: InputMaybe<ListFilterInputTypeOfProductFilterInput>;
  subCategories?: InputMaybe<ListFilterInputTypeOfCategoryFilterInput>;
};

export type CategorySortInput = {
  id?: InputMaybe<SortEnumType>;
  name?: InputMaybe<SortEnumType>;
  parentCategory?: InputMaybe<CategorySortInput>;
  parentCategoryId?: InputMaybe<SortEnumType>;
};

export type Channel = {
  __typename?: 'Channel';
  description?: Maybe<Scalars['String']['output']>;
  id: Scalars['ID']['output'];
  name: Scalars['String']['output'];
};

export type ChannelFilterInput = {
  and?: InputMaybe<Array<ChannelFilterInput>>;
  description?: InputMaybe<StringOperationFilterInput>;
  id?: InputMaybe<UuidOperationFilterInput>;
  name?: InputMaybe<StringOperationFilterInput>;
  or?: InputMaybe<Array<ChannelFilterInput>>;
};

export type ChannelSortInput = {
  description?: InputMaybe<SortEnumType>;
  id?: InputMaybe<SortEnumType>;
  name?: InputMaybe<SortEnumType>;
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

export type CreateDiscountRuleDtoInput = {
  channelId: Scalars['UUID']['input'];
  condition: Scalars['String']['input'];
  discountPercentage: Scalars['Decimal']['input'];
  effectiveFrom: Scalars['DateTime']['input'];
  effectiveTo: Scalars['DateTime']['input'];
  productCategoryId?: InputMaybe<Scalars['UUID']['input']>;
  productVariantId?: InputMaybe<Scalars['UUID']['input']>;
};

export type CreateDiscountRuleError = ResultError;

export type CreateDiscountRuleInput = {
  discountRuleDto: CreateDiscountRuleDtoInput;
};

export type CreateDiscountRulePayload = {
  __typename?: 'CreateDiscountRulePayload';
  discountRule?: Maybe<DiscountRule>;
  errors?: Maybe<Array<CreateDiscountRuleError>>;
};

export type CreateItemDtoInput = {
  expiryDate?: InputMaybe<Scalars['LocalDate']['input']>;
  locationId: Scalars['UUID']['input'];
  productVariantId: Scalars['UUID']['input'];
  quantity: Scalars['Int']['input'];
};

export type CreateItemError = ResultError;

export type CreateItemInput = {
  itemDto: CreateItemDtoInput;
};

export type CreateItemPayload = {
  __typename?: 'CreateItemPayload';
  errors?: Maybe<Array<CreateItemError>>;
  item?: Maybe<Item>;
};

export type CreateLocationDtoInput = {
  address: AddressInput;
  locationTypeId: Scalars['UUID']['input'];
  name: Scalars['String']['input'];
};

export type CreateLocationError = ResultError;

export type CreateLocationInput = {
  locationDto: CreateLocationDtoInput;
};

export type CreateLocationPayload = {
  __typename?: 'CreateLocationPayload';
  errors?: Maybe<Array<CreateLocationError>>;
  location?: Maybe<Location>;
};

export type CreateMovementDtoInput = {
  itemId: Scalars['UUID']['input'];
  locationId: Scalars['UUID']['input'];
  movementDate: Scalars['DateTime']['input'];
  movementTypeId: Scalars['UUID']['input'];
  quantity: Scalars['Int']['input'];
};

export type CreateMovementError = ResultError;

export type CreateMovementInput = {
  movementDto: CreateMovementDtoInput;
};

export type CreateMovementPayload = {
  __typename?: 'CreateMovementPayload';
  errors?: Maybe<Array<CreateMovementError>>;
  movement?: Maybe<Movement>;
};

export type CreateProductDtoInput = {
  categoryIds: Array<Scalars['UUID']['input']>;
  description: Scalars['String']['input'];
  ingredients: Array<Scalars['String']['input']>;
  name: Scalars['String']['input'];
  type: ProductType;
  variantCurrency: Currency;
  variantPrice: Scalars['Decimal']['input'];
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

export type CreateProductPriceDtoInput = {
  channelId: Scalars['UUID']['input'];
  effectiveFrom: Scalars['DateTime']['input'];
  effectiveTo: Scalars['DateTime']['input'];
  price: PriceInput;
  productPriceId: Scalars['UUID']['input'];
  productVariantId: Scalars['UUID']['input'];
};

export type CreateProductPriceError = ResultError;

export type CreateProductPriceInput = {
  productPriceDto: CreateProductPriceDtoInput;
};

export type CreateProductPricePayload = {
  __typename?: 'CreateProductPricePayload';
  errors?: Maybe<Array<CreateProductPriceError>>;
  productPrice?: Maybe<ProductPrice>;
};

export enum Currency {
  Eur = 'EUR',
  Usd = 'USD'
}

export type CurrencyOperationFilterInput = {
  eq?: InputMaybe<Currency>;
  in?: InputMaybe<Array<Currency>>;
  neq?: InputMaybe<Currency>;
  nin?: InputMaybe<Array<Currency>>;
};

export type DateTimeOperationFilterInput = {
  eq?: InputMaybe<Scalars['DateTime']['input']>;
  gt?: InputMaybe<Scalars['DateTime']['input']>;
  gte?: InputMaybe<Scalars['DateTime']['input']>;
  in?: InputMaybe<Array<InputMaybe<Scalars['DateTime']['input']>>>;
  lt?: InputMaybe<Scalars['DateTime']['input']>;
  lte?: InputMaybe<Scalars['DateTime']['input']>;
  neq?: InputMaybe<Scalars['DateTime']['input']>;
  ngt?: InputMaybe<Scalars['DateTime']['input']>;
  ngte?: InputMaybe<Scalars['DateTime']['input']>;
  nin?: InputMaybe<Array<InputMaybe<Scalars['DateTime']['input']>>>;
  nlt?: InputMaybe<Scalars['DateTime']['input']>;
  nlte?: InputMaybe<Scalars['DateTime']['input']>;
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

export type DeleteDiscountRuleError = ResultError;

export type DeleteDiscountRuleInput = {
  id: Scalars['UUID']['input'];
};

export type DeleteDiscountRulePayload = {
  __typename?: 'DeleteDiscountRulePayload';
  boolean?: Maybe<Scalars['Boolean']['output']>;
  errors?: Maybe<Array<DeleteDiscountRuleError>>;
};

export type DeleteItemError = ResultError;

export type DeleteItemInput = {
  id: Scalars['UUID']['input'];
};

export type DeleteItemPayload = {
  __typename?: 'DeleteItemPayload';
  boolean?: Maybe<Scalars['Boolean']['output']>;
  errors?: Maybe<Array<DeleteItemError>>;
};

export type DeleteLocationError = ResultError;

export type DeleteLocationInput = {
  id: Scalars['UUID']['input'];
};

export type DeleteLocationPayload = {
  __typename?: 'DeleteLocationPayload';
  boolean?: Maybe<Scalars['Boolean']['output']>;
  errors?: Maybe<Array<DeleteLocationError>>;
};

export type DeleteMovementError = ResultError;

export type DeleteMovementInput = {
  id: Scalars['UUID']['input'];
};

export type DeleteMovementPayload = {
  __typename?: 'DeleteMovementPayload';
  boolean?: Maybe<Scalars['Boolean']['output']>;
  errors?: Maybe<Array<DeleteMovementError>>;
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

export type DeleteProductPriceError = ResultError;

export type DeleteProductPriceInput = {
  id: Scalars['UUID']['input'];
};

export type DeleteProductPricePayload = {
  __typename?: 'DeleteProductPricePayload';
  boolean?: Maybe<Scalars['Boolean']['output']>;
  errors?: Maybe<Array<DeleteProductPriceError>>;
};

export type DiscountRule = {
  __typename?: 'DiscountRule';
  channel: Channel;
  condition: Scalars['String']['output'];
  discountPercentage: Scalars['Decimal']['output'];
  effectiveFrom: Scalars['DateTime']['output'];
  effectiveTo: Scalars['DateTime']['output'];
  id: Scalars['ID']['output'];
  productCategoryId?: Maybe<Scalars['ID']['output']>;
  productVariantId?: Maybe<Scalars['ID']['output']>;
};

export type DiscountRuleFilterInput = {
  and?: InputMaybe<Array<DiscountRuleFilterInput>>;
  channel?: InputMaybe<ChannelFilterInput>;
  channelId?: InputMaybe<UuidOperationFilterInput>;
  condition?: InputMaybe<StringOperationFilterInput>;
  discountPercentage?: InputMaybe<DecimalOperationFilterInput>;
  effectiveFrom?: InputMaybe<DateTimeOperationFilterInput>;
  effectiveTo?: InputMaybe<DateTimeOperationFilterInput>;
  id?: InputMaybe<UuidOperationFilterInput>;
  or?: InputMaybe<Array<DiscountRuleFilterInput>>;
  productCategoryId?: InputMaybe<UuidOperationFilterInput>;
  productVariantId?: InputMaybe<UuidOperationFilterInput>;
};

export type DiscountRuleSortInput = {
  channel?: InputMaybe<ChannelSortInput>;
  channelId?: InputMaybe<SortEnumType>;
  condition?: InputMaybe<SortEnumType>;
  discountPercentage?: InputMaybe<SortEnumType>;
  effectiveFrom?: InputMaybe<SortEnumType>;
  effectiveTo?: InputMaybe<SortEnumType>;
  id?: InputMaybe<SortEnumType>;
  productCategoryId?: InputMaybe<SortEnumType>;
  productVariantId?: InputMaybe<SortEnumType>;
};

/** A connection to a list of items. */
export type DiscountRulesConnection = {
  __typename?: 'DiscountRulesConnection';
  /** A list of edges. */
  edges?: Maybe<Array<DiscountRulesEdge>>;
  /** A flattened list of the nodes. */
  nodes?: Maybe<Array<DiscountRule>>;
  /** Information to aid in pagination. */
  pageInfo: PageInfo;
};

/** An edge in a connection. */
export type DiscountRulesEdge = {
  __typename?: 'DiscountRulesEdge';
  /** A cursor for use in pagination. */
  cursor: Scalars['String']['output'];
  /** The item at the end of the edge. */
  node: DiscountRule;
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
  currency?: InputMaybe<Currency>;
  description?: InputMaybe<Scalars['String']['input']>;
  id: Scalars['UUID']['input'];
  ingredients?: InputMaybe<Array<Scalars['String']['input']>>;
  isInStock?: InputMaybe<Scalars['Boolean']['input']>;
  isVisible?: InputMaybe<Scalars['Boolean']['input']>;
  name?: InputMaybe<Scalars['String']['input']>;
  price?: InputMaybe<Scalars['Decimal']['input']>;
  type?: InputMaybe<ProductType>;
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

export type IntOperationFilterInput = {
  eq?: InputMaybe<Scalars['Int']['input']>;
  gt?: InputMaybe<Scalars['Int']['input']>;
  gte?: InputMaybe<Scalars['Int']['input']>;
  in?: InputMaybe<Array<InputMaybe<Scalars['Int']['input']>>>;
  lt?: InputMaybe<Scalars['Int']['input']>;
  lte?: InputMaybe<Scalars['Int']['input']>;
  neq?: InputMaybe<Scalars['Int']['input']>;
  ngt?: InputMaybe<Scalars['Int']['input']>;
  ngte?: InputMaybe<Scalars['Int']['input']>;
  nin?: InputMaybe<Array<InputMaybe<Scalars['Int']['input']>>>;
  nlt?: InputMaybe<Scalars['Int']['input']>;
  nlte?: InputMaybe<Scalars['Int']['input']>;
};

export type Item = {
  __typename?: 'Item';
  expiryDate?: Maybe<Scalars['Date']['output']>;
  id: Scalars['ID']['output'];
  location: Location;
  productVariantId: Scalars['ID']['output'];
  quantity: Scalars['Int']['output'];
};

export type ItemFilterInput = {
  and?: InputMaybe<Array<ItemFilterInput>>;
  expiryDate?: InputMaybe<LocalDateOperationFilterInput>;
  id?: InputMaybe<UuidOperationFilterInput>;
  location?: InputMaybe<LocationFilterInput>;
  locationId?: InputMaybe<UuidOperationFilterInput>;
  or?: InputMaybe<Array<ItemFilterInput>>;
  productVariantId?: InputMaybe<UuidOperationFilterInput>;
  quantity?: InputMaybe<IntOperationFilterInput>;
};

export type ItemSortInput = {
  expiryDate?: InputMaybe<SortEnumType>;
  id?: InputMaybe<SortEnumType>;
  location?: InputMaybe<LocationSortInput>;
  locationId?: InputMaybe<SortEnumType>;
  productVariantId?: InputMaybe<SortEnumType>;
  quantity?: InputMaybe<SortEnumType>;
};

/** A connection to a list of items. */
export type ItemsConnection = {
  __typename?: 'ItemsConnection';
  /** A list of edges. */
  edges?: Maybe<Array<ItemsEdge>>;
  /** A flattened list of the nodes. */
  nodes?: Maybe<Array<Item>>;
  /** Information to aid in pagination. */
  pageInfo: PageInfo;
};

/** An edge in a connection. */
export type ItemsEdge = {
  __typename?: 'ItemsEdge';
  /** A cursor for use in pagination. */
  cursor: Scalars['String']['output'];
  /** The item at the end of the edge. */
  node: Item;
};

export type KeyValuePairOfStringAndStringInput = {
  key: Scalars['String']['input'];
  value: Scalars['String']['input'];
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

export type ListFilterInputTypeOfProductImageFilterInput = {
  all?: InputMaybe<ProductImageFilterInput>;
  any?: InputMaybe<Scalars['Boolean']['input']>;
  none?: InputMaybe<ProductImageFilterInput>;
  some?: InputMaybe<ProductImageFilterInput>;
};

export type ListFilterInputTypeOfProductVariantAttributeFilterInput = {
  all?: InputMaybe<ProductVariantAttributeFilterInput>;
  any?: InputMaybe<Scalars['Boolean']['input']>;
  none?: InputMaybe<ProductVariantAttributeFilterInput>;
  some?: InputMaybe<ProductVariantAttributeFilterInput>;
};

export type ListFilterInputTypeOfProductVariantFilterInput = {
  all?: InputMaybe<ProductVariantFilterInput>;
  any?: InputMaybe<Scalars['Boolean']['input']>;
  none?: InputMaybe<ProductVariantFilterInput>;
  some?: InputMaybe<ProductVariantFilterInput>;
};

export type LocalDateOperationFilterInput = {
  eq?: InputMaybe<Scalars['LocalDate']['input']>;
  gt?: InputMaybe<Scalars['LocalDate']['input']>;
  gte?: InputMaybe<Scalars['LocalDate']['input']>;
  in?: InputMaybe<Array<InputMaybe<Scalars['LocalDate']['input']>>>;
  lt?: InputMaybe<Scalars['LocalDate']['input']>;
  lte?: InputMaybe<Scalars['LocalDate']['input']>;
  neq?: InputMaybe<Scalars['LocalDate']['input']>;
  ngt?: InputMaybe<Scalars['LocalDate']['input']>;
  ngte?: InputMaybe<Scalars['LocalDate']['input']>;
  nin?: InputMaybe<Array<InputMaybe<Scalars['LocalDate']['input']>>>;
  nlt?: InputMaybe<Scalars['LocalDate']['input']>;
  nlte?: InputMaybe<Scalars['LocalDate']['input']>;
};

export type Location = {
  __typename?: 'Location';
  address: Address;
  id: Scalars['ID']['output'];
  locationType: LocationType;
  name: Scalars['String']['output'];
};

export type LocationFilterInput = {
  address?: InputMaybe<AddressFilterInput>;
  and?: InputMaybe<Array<LocationFilterInput>>;
  id?: InputMaybe<UuidOperationFilterInput>;
  locationType?: InputMaybe<LocationTypeFilterInput>;
  locationTypeId?: InputMaybe<UuidOperationFilterInput>;
  name?: InputMaybe<StringOperationFilterInput>;
  or?: InputMaybe<Array<LocationFilterInput>>;
};

export type LocationSortInput = {
  address?: InputMaybe<AddressSortInput>;
  id?: InputMaybe<SortEnumType>;
  locationType?: InputMaybe<LocationTypeSortInput>;
  locationTypeId?: InputMaybe<SortEnumType>;
  name?: InputMaybe<SortEnumType>;
};

export type LocationType = {
  __typename?: 'LocationType';
  description?: Maybe<Scalars['String']['output']>;
  id: Scalars['ID']['output'];
  name: Scalars['String']['output'];
};

export type LocationTypeFilterInput = {
  and?: InputMaybe<Array<LocationTypeFilterInput>>;
  description?: InputMaybe<StringOperationFilterInput>;
  id?: InputMaybe<UuidOperationFilterInput>;
  name?: InputMaybe<StringOperationFilterInput>;
  or?: InputMaybe<Array<LocationTypeFilterInput>>;
};

export type LocationTypeSortInput = {
  description?: InputMaybe<SortEnumType>;
  id?: InputMaybe<SortEnumType>;
  name?: InputMaybe<SortEnumType>;
};

/** A connection to a list of items. */
export type LocationsConnection = {
  __typename?: 'LocationsConnection';
  /** A list of edges. */
  edges?: Maybe<Array<LocationsEdge>>;
  /** A flattened list of the nodes. */
  nodes?: Maybe<Array<Location>>;
  /** Information to aid in pagination. */
  pageInfo: PageInfo;
};

/** An edge in a connection. */
export type LocationsEdge = {
  __typename?: 'LocationsEdge';
  /** A cursor for use in pagination. */
  cursor: Scalars['String']['output'];
  /** The item at the end of the edge. */
  node: Location;
};

export type Movement = {
  __typename?: 'Movement';
  id: Scalars['ID']['output'];
  item: Item;
  location: Location;
  movementDate: Scalars['DateTime']['output'];
  movementType: MovementType;
  quantity: Scalars['Int']['output'];
};

export type MovementFilterInput = {
  and?: InputMaybe<Array<MovementFilterInput>>;
  id?: InputMaybe<UuidOperationFilterInput>;
  item?: InputMaybe<ItemFilterInput>;
  itemId?: InputMaybe<UuidOperationFilterInput>;
  location?: InputMaybe<LocationFilterInput>;
  locationId?: InputMaybe<UuidOperationFilterInput>;
  movementDate?: InputMaybe<DateTimeOperationFilterInput>;
  movementType?: InputMaybe<MovementTypeFilterInput>;
  movementTypeId?: InputMaybe<UuidOperationFilterInput>;
  or?: InputMaybe<Array<MovementFilterInput>>;
  quantity?: InputMaybe<IntOperationFilterInput>;
};

export type MovementSortInput = {
  id?: InputMaybe<SortEnumType>;
  item?: InputMaybe<ItemSortInput>;
  itemId?: InputMaybe<SortEnumType>;
  location?: InputMaybe<LocationSortInput>;
  locationId?: InputMaybe<SortEnumType>;
  movementDate?: InputMaybe<SortEnumType>;
  movementType?: InputMaybe<MovementTypeSortInput>;
  movementTypeId?: InputMaybe<SortEnumType>;
  quantity?: InputMaybe<SortEnumType>;
};

export type MovementType = {
  __typename?: 'MovementType';
  description?: Maybe<Scalars['String']['output']>;
  id: Scalars['ID']['output'];
  name: Scalars['String']['output'];
};

export type MovementTypeFilterInput = {
  and?: InputMaybe<Array<MovementTypeFilterInput>>;
  description?: InputMaybe<StringOperationFilterInput>;
  id?: InputMaybe<UuidOperationFilterInput>;
  name?: InputMaybe<StringOperationFilterInput>;
  or?: InputMaybe<Array<MovementTypeFilterInput>>;
};

export type MovementTypeSortInput = {
  description?: InputMaybe<SortEnumType>;
  id?: InputMaybe<SortEnumType>;
  name?: InputMaybe<SortEnumType>;
};

/** A connection to a list of items. */
export type MovementsConnection = {
  __typename?: 'MovementsConnection';
  /** A list of edges. */
  edges?: Maybe<Array<MovementsEdge>>;
  /** A flattened list of the nodes. */
  nodes?: Maybe<Array<Movement>>;
  /** Information to aid in pagination. */
  pageInfo: PageInfo;
};

/** An edge in a connection. */
export type MovementsEdge = {
  __typename?: 'MovementsEdge';
  /** A cursor for use in pagination. */
  cursor: Scalars['String']['output'];
  /** The item at the end of the edge. */
  node: Movement;
};

export type Mutation = {
  __typename?: 'Mutation';
  addProductVariant: AddProductVariantPayload;
  addProductVariantAttribute: AddProductVariantAttributePayload;
  createCategory: CreateCategoryPayload;
  createDiscountRule: CreateDiscountRulePayload;
  createItem: CreateItemPayload;
  createLocation: CreateLocationPayload;
  createMovement: CreateMovementPayload;
  createProduct: CreateProductPayload;
  createProductPrice: CreateProductPricePayload;
  deleteCategory: DeleteCategoryPayload;
  deleteDiscountRule: DeleteDiscountRulePayload;
  deleteItem: DeleteItemPayload;
  deleteLocation: DeleteLocationPayload;
  deleteMovement: DeleteMovementPayload;
  deleteProduct: DeleteProductPayload;
  deleteProductPrice: DeleteProductPricePayload;
  editCategory: EditCategoryPayload;
  editProduct: EditProductPayload;
  updateDiscountRule: UpdateDiscountRulePayload;
  updateItem: UpdateItemPayload;
  updateLocation: UpdateLocationPayload;
  updateMovement: UpdateMovementPayload;
  updateProductPrice: UpdateProductPricePayload;
};


export type MutationAddProductVariantArgs = {
  input: AddProductVariantInput;
};


export type MutationAddProductVariantAttributeArgs = {
  input: AddProductVariantAttributeInput;
};


export type MutationCreateCategoryArgs = {
  input: CreateCategoryInput;
};


export type MutationCreateDiscountRuleArgs = {
  input: CreateDiscountRuleInput;
};


export type MutationCreateItemArgs = {
  input: CreateItemInput;
};


export type MutationCreateLocationArgs = {
  input: CreateLocationInput;
};


export type MutationCreateMovementArgs = {
  input: CreateMovementInput;
};


export type MutationCreateProductArgs = {
  input: CreateProductInput;
};


export type MutationCreateProductPriceArgs = {
  input: CreateProductPriceInput;
};


export type MutationDeleteCategoryArgs = {
  input: DeleteCategoryInput;
};


export type MutationDeleteDiscountRuleArgs = {
  input: DeleteDiscountRuleInput;
};


export type MutationDeleteItemArgs = {
  input: DeleteItemInput;
};


export type MutationDeleteLocationArgs = {
  input: DeleteLocationInput;
};


export type MutationDeleteMovementArgs = {
  input: DeleteMovementInput;
};


export type MutationDeleteProductArgs = {
  input: DeleteProductInput;
};


export type MutationDeleteProductPriceArgs = {
  input: DeleteProductPriceInput;
};


export type MutationEditCategoryArgs = {
  input: EditCategoryInput;
};


export type MutationEditProductArgs = {
  input: EditProductInput;
};


export type MutationUpdateDiscountRuleArgs = {
  input: UpdateDiscountRuleInput;
};


export type MutationUpdateItemArgs = {
  input: UpdateItemInput;
};


export type MutationUpdateLocationArgs = {
  input: UpdateLocationInput;
};


export type MutationUpdateMovementArgs = {
  input: UpdateMovementInput;
};


export type MutationUpdateProductPriceArgs = {
  input: UpdateProductPriceInput;
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
  currency: Currency;
};

export type PriceFilterInput = {
  amount?: InputMaybe<DecimalOperationFilterInput>;
  and?: InputMaybe<Array<PriceFilterInput>>;
  currency?: InputMaybe<CurrencyOperationFilterInput>;
  or?: InputMaybe<Array<PriceFilterInput>>;
};

export type PriceInput = {
  amount: Scalars['Decimal']['input'];
  currency: Currency;
};

export type PriceRangeBucket = {
  __typename?: 'PriceRangeBucket';
  count: Scalars['Int']['output'];
  from: Scalars['Decimal']['output'];
  label: Scalars['String']['output'];
  to: Scalars['Decimal']['output'];
};

export type PriceRangeFilter = {
  __typename?: 'PriceRangeFilter';
  buckets: Array<PriceRangeBucket>;
  maxPrice?: Maybe<Scalars['Float']['output']>;
  minPrice?: Maybe<Scalars['Float']['output']>;
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
  isInStock: Scalars['Boolean']['output'];
  isVisible: Scalars['Boolean']['output'];
  name: Scalars['String']['output'];
  type: ProductType;
  variants?: Maybe<Array<Maybe<ProductVariant>>>;
};

export type ProductDocument = {
  __typename?: 'ProductDocument';
  categories: Array<CategoryDocument>;
  description: Scalars['String']['output'];
  id: Scalars['UUID']['output'];
  isInStock: Scalars['Boolean']['output'];
  isVisible: Scalars['Boolean']['output'];
  name: Scalars['String']['output'];
  type: ProductType;
  variants: Array<ProductVariantDocument>;
};

export type ProductFilterInput = {
  and?: InputMaybe<Array<ProductFilterInput>>;
  categories?: InputMaybe<ListFilterInputTypeOfCategoryFilterInput>;
  description?: InputMaybe<StringOperationFilterInput>;
  id?: InputMaybe<UuidOperationFilterInput>;
  isInStock?: InputMaybe<BooleanOperationFilterInput>;
  isVisible?: InputMaybe<BooleanOperationFilterInput>;
  name?: InputMaybe<StringOperationFilterInput>;
  or?: InputMaybe<Array<ProductFilterInput>>;
  type?: InputMaybe<ProductTypeOperationFilterInput>;
  variants?: InputMaybe<ListFilterInputTypeOfProductVariantFilterInput>;
};

export type ProductImage = {
  __typename?: 'ProductImage';
  altText?: Maybe<Scalars['String']['output']>;
  id: Scalars['ID']['output'];
  imageUrl: Scalars['String']['output'];
  productVariant?: Maybe<Array<Maybe<ProductVariant>>>;
  sortOrder: Scalars['Int']['output'];
};

export type ProductImageFilterInput = {
  altText?: InputMaybe<StringOperationFilterInput>;
  and?: InputMaybe<Array<ProductImageFilterInput>>;
  id?: InputMaybe<UuidOperationFilterInput>;
  imageUrl?: InputMaybe<StringOperationFilterInput>;
  or?: InputMaybe<Array<ProductImageFilterInput>>;
  productVariant?: InputMaybe<ProductVariantFilterInput>;
  productVariantId?: InputMaybe<UuidOperationFilterInput>;
  sortOrder?: InputMaybe<IntOperationFilterInput>;
};

export type ProductPrice = {
  __typename?: 'ProductPrice';
  channel: Channel;
  effectiveFrom: Scalars['DateTime']['output'];
  effectiveTo: Scalars['DateTime']['output'];
  id: Scalars['ID']['output'];
  price: Price;
  productVariantId: Scalars['ID']['output'];
};

export type ProductPriceFilterInput = {
  and?: InputMaybe<Array<ProductPriceFilterInput>>;
  channel?: InputMaybe<ChannelFilterInput>;
  channelId?: InputMaybe<UuidOperationFilterInput>;
  effectiveFrom?: InputMaybe<DateTimeOperationFilterInput>;
  effectiveTo?: InputMaybe<DateTimeOperationFilterInput>;
  id?: InputMaybe<UuidOperationFilterInput>;
  or?: InputMaybe<Array<ProductPriceFilterInput>>;
  price?: InputMaybe<PriceFilterInput>;
  productVariantId?: InputMaybe<UuidOperationFilterInput>;
};

export type ProductPriceSortInput = {
  channel?: InputMaybe<ChannelSortInput>;
  channelId?: InputMaybe<SortEnumType>;
  effectiveFrom?: InputMaybe<SortEnumType>;
  effectiveTo?: InputMaybe<SortEnumType>;
  id?: InputMaybe<SortEnumType>;
  price?: InputMaybe<PriceSortInput>;
  productVariantId?: InputMaybe<SortEnumType>;
};

/** A connection to a list of items. */
export type ProductPricesConnection = {
  __typename?: 'ProductPricesConnection';
  /** A list of edges. */
  edges?: Maybe<Array<ProductPricesEdge>>;
  /** A flattened list of the nodes. */
  nodes?: Maybe<Array<ProductPrice>>;
  /** Information to aid in pagination. */
  pageInfo: PageInfo;
};

/** An edge in a connection. */
export type ProductPricesEdge = {
  __typename?: 'ProductPricesEdge';
  /** A cursor for use in pagination. */
  cursor: Scalars['String']['output'];
  /** The item at the end of the edge. */
  node: ProductPrice;
};

export type ProductSortInput = {
  description?: InputMaybe<SortEnumType>;
  id?: InputMaybe<SortEnumType>;
  isInStock?: InputMaybe<SortEnumType>;
  isVisible?: InputMaybe<SortEnumType>;
  name?: InputMaybe<SortEnumType>;
  type?: InputMaybe<SortEnumType>;
};

export enum ProductType {
  Offline = 'OFFLINE',
  Online = 'ONLINE'
}

export type ProductTypeOperationFilterInput = {
  eq?: InputMaybe<ProductType>;
  in?: InputMaybe<Array<ProductType>>;
  neq?: InputMaybe<ProductType>;
  nin?: InputMaybe<Array<ProductType>>;
};

export type ProductVariant = {
  __typename?: 'ProductVariant';
  id: Scalars['ID']['output'];
  images: Array<Maybe<ProductImage>>;
  isInStock: Scalars['Boolean']['output'];
  isVisible: Scalars['Boolean']['output'];
  price: Price;
  product: Product;
  variantAttributes: Array<Maybe<ProductVariantAttribute>>;
};

export type ProductVariantAttribute = {
  __typename?: 'ProductVariantAttribute';
  attributeDefinition?: Maybe<VariantAttributeDefinition>;
  id: Scalars['ID']['output'];
  unitsOfMeasure?: Maybe<UnitsOfMeasure>;
  value?: Maybe<Scalars['String']['output']>;
};

export type ProductVariantAttributeDocument = {
  __typename?: 'ProductVariantAttributeDocument';
  attributeName: Scalars['String']['output'];
  id: Scalars['UUID']['output'];
  unitsOfMeasureAbbreviation?: Maybe<Scalars['String']['output']>;
  unitsOfMeasureName?: Maybe<Scalars['String']['output']>;
  value: Scalars['String']['output'];
};

export type ProductVariantAttributeFilterInput = {
  and?: InputMaybe<Array<ProductVariantAttributeFilterInput>>;
  attributeDefinition?: InputMaybe<VariantAttributeDefinitionFilterInput>;
  attributeDefinitionId?: InputMaybe<UuidOperationFilterInput>;
  id?: InputMaybe<UuidOperationFilterInput>;
  or?: InputMaybe<Array<ProductVariantAttributeFilterInput>>;
  unitsOfMeasure?: InputMaybe<UnitsOfMeasureFilterInput>;
  unitsOfMeasureId?: InputMaybe<UuidOperationFilterInput>;
  value?: InputMaybe<StringOperationFilterInput>;
};

export type ProductVariantDocument = {
  __typename?: 'ProductVariantDocument';
  id: Scalars['UUID']['output'];
  isInStock: Scalars['Boolean']['output'];
  isVisible: Scalars['Boolean']['output'];
  priceAmount: Scalars['Decimal']['output'];
  priceCurrency: Scalars['String']['output'];
  variantAttributes: Array<ProductVariantAttributeDocument>;
};

export type ProductVariantFilterInput = {
  and?: InputMaybe<Array<ProductVariantFilterInput>>;
  id?: InputMaybe<UuidOperationFilterInput>;
  images?: InputMaybe<ListFilterInputTypeOfProductImageFilterInput>;
  isInStock?: InputMaybe<BooleanOperationFilterInput>;
  isVisible?: InputMaybe<BooleanOperationFilterInput>;
  or?: InputMaybe<Array<ProductVariantFilterInput>>;
  price?: InputMaybe<PriceFilterInput>;
  product?: InputMaybe<ProductFilterInput>;
  productId?: InputMaybe<UuidOperationFilterInput>;
  variantAttributes?: InputMaybe<ListFilterInputTypeOfProductVariantAttributeFilterInput>;
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
  discountRules?: Maybe<DiscountRulesConnection>;
  items?: Maybe<ItemsConnection>;
  locations?: Maybe<LocationsConnection>;
  movements?: Maybe<MovementsConnection>;
  productPrices?: Maybe<ProductPricesConnection>;
  products?: Maybe<ProductsConnection>;
  searchProducts: Array<ProductDocument>;
  searchProductsByName: Array<ProductDocument>;
  searchProductsWithFilters: SearchProductsWithFiltersResponse;
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


export type QueryDiscountRulesArgs = {
  after?: InputMaybe<Scalars['String']['input']>;
  before?: InputMaybe<Scalars['String']['input']>;
  first?: InputMaybe<Scalars['Int']['input']>;
  last?: InputMaybe<Scalars['Int']['input']>;
  order?: InputMaybe<Array<DiscountRuleSortInput>>;
  where?: InputMaybe<DiscountRuleFilterInput>;
};


export type QueryItemsArgs = {
  after?: InputMaybe<Scalars['String']['input']>;
  before?: InputMaybe<Scalars['String']['input']>;
  first?: InputMaybe<Scalars['Int']['input']>;
  last?: InputMaybe<Scalars['Int']['input']>;
  order?: InputMaybe<Array<ItemSortInput>>;
  where?: InputMaybe<ItemFilterInput>;
};


export type QueryLocationsArgs = {
  after?: InputMaybe<Scalars['String']['input']>;
  before?: InputMaybe<Scalars['String']['input']>;
  first?: InputMaybe<Scalars['Int']['input']>;
  last?: InputMaybe<Scalars['Int']['input']>;
  order?: InputMaybe<Array<LocationSortInput>>;
  where?: InputMaybe<LocationFilterInput>;
};


export type QueryMovementsArgs = {
  after?: InputMaybe<Scalars['String']['input']>;
  before?: InputMaybe<Scalars['String']['input']>;
  first?: InputMaybe<Scalars['Int']['input']>;
  last?: InputMaybe<Scalars['Int']['input']>;
  order?: InputMaybe<Array<MovementSortInput>>;
  where?: InputMaybe<MovementFilterInput>;
};


export type QueryProductPricesArgs = {
  after?: InputMaybe<Scalars['String']['input']>;
  before?: InputMaybe<Scalars['String']['input']>;
  first?: InputMaybe<Scalars['Int']['input']>;
  last?: InputMaybe<Scalars['Int']['input']>;
  order?: InputMaybe<Array<ProductPriceSortInput>>;
  where?: InputMaybe<ProductPriceFilterInput>;
};


export type QueryProductsArgs = {
  after?: InputMaybe<Scalars['String']['input']>;
  before?: InputMaybe<Scalars['String']['input']>;
  first?: InputMaybe<Scalars['Int']['input']>;
  last?: InputMaybe<Scalars['Int']['input']>;
  order?: InputMaybe<Array<ProductSortInput>>;
  where?: InputMaybe<ProductFilterInput>;
};


export type QuerySearchProductsArgs = {
  categoryIds?: InputMaybe<Array<Scalars['UUID']['input']>>;
  inStockOnly?: InputMaybe<Scalars['Boolean']['input']>;
  maxPrice?: InputMaybe<Scalars['Float']['input']>;
  minPrice?: InputMaybe<Scalars['Float']['input']>;
  productName: Scalars['String']['input'];
  skip?: InputMaybe<Scalars['Int']['input']>;
  take?: InputMaybe<Scalars['Int']['input']>;
  variantAttributes?: InputMaybe<Array<KeyValuePairOfStringAndStringInput>>;
};


export type QuerySearchProductsByNameArgs = {
  productName: Scalars['String']['input'];
  skip: Scalars['Int']['input'];
  take: Scalars['Int']['input'];
};


export type QuerySearchProductsWithFiltersArgs = {
  categoryIds?: InputMaybe<Array<Scalars['UUID']['input']>>;
  inStockOnly?: InputMaybe<Scalars['Boolean']['input']>;
  maxPrice?: InputMaybe<Scalars['Float']['input']>;
  minPrice?: InputMaybe<Scalars['Float']['input']>;
  productName?: InputMaybe<Scalars['String']['input']>;
  skip: Scalars['Int']['input'];
  sortBy: SortBy;
  sortOrder?: InputMaybe<SortOrder>;
  take: Scalars['Int']['input'];
  variantAttributesIds?: InputMaybe<Array<Scalars['UUID']['input']>>;
};

export type Recipe = {
  __typename?: 'Recipe';
  description: Scalars['String']['output'];
  id: Scalars['ID']['output'];
  ingredients: Array<Maybe<RecipeIngredient>>;
  instructions: Scalars['String']['output'];
  isPublished: Scalars['Boolean']['output'];
  name: Scalars['String']['output'];
  preparationTimeInMinutes: Scalars['Int']['output'];
  productVariant?: Maybe<ProductVariant>;
};

export type RecipeIngredient = {
  __typename?: 'RecipeIngredient';
  id: Scalars['ID']['output'];
  productVariant?: Maybe<ProductVariant>;
  quantity: Scalars['Float']['output'];
  recipe: Recipe;
  unitsOfMeasure?: Maybe<UnitsOfMeasure>;
};

export type ResultError = Error & {
  __typename?: 'ResultError';
  code: Scalars['String']['output'];
  message: Scalars['String']['output'];
};

export type SearchFilters = {
  __typename?: 'SearchFilters';
  attributes: Array<AttributeFilter>;
  categories: Array<CategoryFilter>;
  priceRange: PriceRangeFilter;
};

export type SearchProductsWithFiltersResponse = {
  __typename?: 'SearchProductsWithFiltersResponse';
  filters?: Maybe<SearchFilters>;
  products: Array<ProductDocument>;
  totalCount: Scalars['Long']['output'];
};

export enum SortBy {
  Name = 'NAME',
  Popularity = 'POPULARITY',
  Price = 'PRICE'
}

export enum SortEnumType {
  Asc = 'ASC',
  Desc = 'DESC'
}

export enum SortOrder {
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

export type UnitsOfMeasure = {
  __typename?: 'UnitsOfMeasure';
  abbreviation: Scalars['String']['output'];
  description?: Maybe<Scalars['String']['output']>;
  id: Scalars['ID']['output'];
  name: Scalars['String']['output'];
};

export type UnitsOfMeasureFilterInput = {
  abbreviation?: InputMaybe<StringOperationFilterInput>;
  and?: InputMaybe<Array<UnitsOfMeasureFilterInput>>;
  description?: InputMaybe<StringOperationFilterInput>;
  id?: InputMaybe<UuidOperationFilterInput>;
  name?: InputMaybe<StringOperationFilterInput>;
  or?: InputMaybe<Array<UnitsOfMeasureFilterInput>>;
};

export type UpdateDiscountRuleDtoInput = {
  channelId?: InputMaybe<Scalars['UUID']['input']>;
  condition?: InputMaybe<Scalars['String']['input']>;
  discountPercentage?: InputMaybe<Scalars['Decimal']['input']>;
  discountRuleId: Scalars['UUID']['input'];
  effectiveFrom?: InputMaybe<Scalars['DateTime']['input']>;
  effectiveTo?: InputMaybe<Scalars['DateTime']['input']>;
  productCategoryId?: InputMaybe<Scalars['UUID']['input']>;
  productVariantId?: InputMaybe<Scalars['UUID']['input']>;
};

export type UpdateDiscountRuleError = ResultError;

export type UpdateDiscountRuleInput = {
  discountRuleDto: UpdateDiscountRuleDtoInput;
};

export type UpdateDiscountRulePayload = {
  __typename?: 'UpdateDiscountRulePayload';
  boolean?: Maybe<Scalars['Boolean']['output']>;
  errors?: Maybe<Array<UpdateDiscountRuleError>>;
};

export type UpdateItemDtoInput = {
  expiryDate?: InputMaybe<Scalars['LocalDate']['input']>;
  itemId: Scalars['UUID']['input'];
  locationId?: InputMaybe<Scalars['UUID']['input']>;
  productVariantId?: InputMaybe<Scalars['UUID']['input']>;
  quantity?: InputMaybe<Scalars['Int']['input']>;
};

export type UpdateItemError = ResultError;

export type UpdateItemInput = {
  itemDto: UpdateItemDtoInput;
};

export type UpdateItemPayload = {
  __typename?: 'UpdateItemPayload';
  boolean?: Maybe<Scalars['Boolean']['output']>;
  errors?: Maybe<Array<UpdateItemError>>;
};

export type UpdateLocationDtoInput = {
  address?: InputMaybe<AddressInput>;
  locationId: Scalars['UUID']['input'];
  locationTypeId?: InputMaybe<Scalars['UUID']['input']>;
  name?: InputMaybe<Scalars['String']['input']>;
};

export type UpdateLocationError = ResultError;

export type UpdateLocationInput = {
  locationDto: UpdateLocationDtoInput;
};

export type UpdateLocationPayload = {
  __typename?: 'UpdateLocationPayload';
  boolean?: Maybe<Scalars['Boolean']['output']>;
  errors?: Maybe<Array<UpdateLocationError>>;
};

export type UpdateMovementDtoInput = {
  itemId?: InputMaybe<Scalars['UUID']['input']>;
  locationId?: InputMaybe<Scalars['UUID']['input']>;
  movementDate?: InputMaybe<Scalars['DateTime']['input']>;
  movementId: Scalars['UUID']['input'];
  movementTypeId?: InputMaybe<Scalars['UUID']['input']>;
  quantity?: InputMaybe<Scalars['Int']['input']>;
};

export type UpdateMovementError = ResultError;

export type UpdateMovementInput = {
  movementDto: UpdateMovementDtoInput;
};

export type UpdateMovementPayload = {
  __typename?: 'UpdateMovementPayload';
  boolean?: Maybe<Scalars['Boolean']['output']>;
  errors?: Maybe<Array<UpdateMovementError>>;
};

export type UpdateProductPriceDtoInput = {
  channelId?: InputMaybe<Scalars['UUID']['input']>;
  effectiveFrom?: InputMaybe<Scalars['DateTime']['input']>;
  effectiveTo?: InputMaybe<Scalars['DateTime']['input']>;
  price?: InputMaybe<PriceInput>;
  productPriceId: Scalars['UUID']['input'];
  productVariantId?: InputMaybe<Scalars['UUID']['input']>;
};

export type UpdateProductPriceError = ResultError;

export type UpdateProductPriceInput = {
  productPriceDto: UpdateProductPriceDtoInput;
};

export type UpdateProductPricePayload = {
  __typename?: 'UpdateProductPricePayload';
  boolean?: Maybe<Scalars['Boolean']['output']>;
  errors?: Maybe<Array<UpdateProductPriceError>>;
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

export type VariantAttributeDefinition = {
  __typename?: 'VariantAttributeDefinition';
  description?: Maybe<Scalars['String']['output']>;
  id: Scalars['ID']['output'];
  name: Scalars['String']['output'];
};

export type VariantAttributeDefinitionFilterInput = {
  and?: InputMaybe<Array<VariantAttributeDefinitionFilterInput>>;
  description?: InputMaybe<StringOperationFilterInput>;
  id?: InputMaybe<UuidOperationFilterInput>;
  name?: InputMaybe<StringOperationFilterInput>;
  or?: InputMaybe<Array<VariantAttributeDefinitionFilterInput>>;
};
