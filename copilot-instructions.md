# Microservices Cafe - GitHub Copilot Instructions

## Project Overview
This is a full-stack microservices e-commerce application for a coffee shop built with:
- **Backend**: .NET 8 microservices with HotChocolate GraphQL Federation (Fusion Gateway)
- **Frontend**: Next.js 15 with TypeScript, Apollo Client, and next-intl for internationalization
- **Infrastructure**: Docker Compose, RabbitMQ, MS SQL Server, Elasticsearch

---

## Architecture & Technology Stack

### Backend (.NET 8 Microservices)
- **Architecture Pattern**: Clean Architecture (Domain, Application, Infrastructure, API layers)
- **API Layer**: HotChocolate GraphQL v15 with Federation
- **Gateway**: HotChocolate Fusion Gateway for unified GraphQL schema
- **Authentication**: OpenIddict (OAuth 2.0/OpenID Connect)
- **Database**: Entity Framework Core 8 with SQL Server
- **Message Broker**: MassTransit with RabbitMQ
- **Search**: Elasticsearch 
- **Validation**: FluentValidation 
- **Mapping**: Mapster 
- **CQRS**: MediatR 
- **Background Jobs**: Quartz.NET 
- **Resilience**: Polly 
- **Package Management**: Central Package Management (Directory.Packages.props)

### Microservices
1. **Auth.Server** - Authentication and authorization service
2. **Gateway.API** - GraphQL Fusion Gateway (federated schema)
3. **Products.API** - Product catalog, categories, variants, recipes
4. **Inventory.API** - Stock management
5. **Price.API** - Pricing management
6. **User.API** - User profile management

### Frontend (Next.js 15)
- **Framework**: Next.js 15.1.3 with App Router
- **Language**: TypeScript 5
- **GraphQL Client**: Apollo Client 3.12.5 with experimental Next.js support
- **Authentication**: NextAuth.js 5.0.0 (beta)
- **Internationalization**: next-intl 4.0.2
- **UI Components**: Radix UI primitives with custom components
- **Styling**: Tailwind CSS 4.0.5
- **Forms**: React Hook Form 7.54.2 with Zod 4.0.5
- **Code Generation**: GraphQL Code Generator for TypeScript types
- **State Management**: React Context + nuqs 2.6.0 for URL state
- **Theme**: next-themes 0.4.6 (light/dark mode)

---

## Code Style & Conventions

### Backend (.NET)

#### Project Structure
- Follow Clean Architecture layering strictly:
  - `Domain`: Entities, Value Objects, Domain Events, Enums
  - `Application`: CQRS (Commands/Queries), Event Handlers, DTOs, Validators
  - `Infrastructure`: DbContext, Repositories, External Services
  - `API`: GraphQL Types, Mutations, Queries, Program.cs
  - `Shared`: Cross-cutting concerns, abstractions, primitives

#### Naming Conventions
- **Commands**: `{Action}{Entity}Command` (e.g., `CreateProductCommand`)
- **Command Handlers**: `{CommandName}Handler` (e.g., `CreateProductCommandHandler`)
- **Queries**: `Get{Entity}Query`, `Search{Entity}Query`
- **Query Handlers**: `{QueryName}QueryHandler`
- **Domain Events**: `{Entity}{Action}Event` (e.g., `ProductCreatedEvent`)
- **Event Handlers**: `{EventName}EventHandler`
- **GraphQL Types**: `{Entity}Type` (e.g., `ProductType`)
- **GraphQL Mutations**: `{Entity}Mutations` (e.g., `ProductsMutations`)
- **Validators**: `{CommandOrQuery}Validator`

#### CQRS Pattern with MediatR
- All commands/queries implement `IRequest<T>` or custom messaging interfaces
- Use `IResultCommand<T>` and `IQuery<T>` from Shared abstractions
- Commands return `Result<T>` or `Result` for error handling
- Queries can return direct types or `IEnumerable<T>`
- Always use `CancellationToken` in async methods

```csharp
// Command Example
public record CreateProductCommand(string Name, string Description) : IResultCommand<Product>;

public class CreateProductCommandHandler(IProductsDbContext dbContext) 
    : IRequestHandler<CreateProductCommand, Result<Product>>
{
    public async Task<Result<Product>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        // Implementation
    }
}
```

#### GraphQL with HotChocolate
- Use **ObjectType<T>** for entity types with explicit configuration
- Use **Mutations** classes for grouping related mutations
- Apply `[Authorize]` attribute for protected operations
- Use `[Error<TError>]` for explicit error types
- Return `Task<Result<T>>` from mutations for consistent error handling
- Use Mutation Conventions for standardized responses

```csharp
[ObjectType<Product>]
public static partial class ProductType
{
    static partial void Configure(IObjectTypeDescriptor<Product> descriptor)
    {
        descriptor.Field(p => p.Variants)
            .ResolveWith<Resolvers>(r => r.GetVariants(default!, default));
    }
}

public class ProductsMutations
{
    [Authorize]
    [Error<NotFoundError>]
    public async Task<Result<Product>> CreateProduct(
        CreateProductCommand command,
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
    {
        return await mediator.Send(command, cancellationToken);
    }
}
```

#### Entity Framework Core
- Entities inherit from `BaseEntity` or `AggregateRoot` from Shared
- Use Fluent API configuration in `EntityTypeConfiguration<T>`
- Override `SaveChangesAsync` to dispatch domain events
- Use repository pattern when needed (though DbContext acts as UoW)
- Always use async methods with proper cancellation token handling

#### Domain Events
- Events implement `IDomainEvent` (which extends `INotification`)
- Raised in entity methods via `RaiseDomainEvent()`
- Handled by `INotificationHandler<TEvent>`
- Use for triggering side effects (e.g., Elasticsearch indexing, message publishing)

```csharp
public class ProductCreatedEvent : IDomainEvent
{
    public Product Product { get; }
}

public class ProductCreatedEventHandler(IElasticsearchService elasticsearchService) 
    : INotificationHandler<ProductCreatedEvent>
{
    public async Task Handle(ProductCreatedEvent notification, CancellationToken cancellationToken)
    {
        await elasticsearchService.IndexDocumentAsync(
            ElasticIndex.Products, 
            notification.Product, 
            notification.Product.Id, 
            cancellationToken);
    }
}
```

#### Validation
- Use FluentValidation for all commands and queries
- Create `{CommandName}Validator : AbstractValidator<TCommand>`
- Validators are executed via `ValidationBehavior` pipeline
- Return validation errors as `Result.Failure(errors)`

#### Background Jobs & Outbox Pattern
- Use Quartz.NET for scheduled jobs
- Implement `ProcessOutboxMessagesJob` for reliable message publishing
- Store integration events in outbox table before publishing to RabbitMQ

#### Elasticsearch Integration
- Use `IElasticsearchService` abstraction from Shared
- Index documents on entity create/update via domain event handlers
- Define `ElasticIndex` enum for index names
- Initialize indices on app startup via `InitializeElasticsearchIndicesAsync`

#### Dependency Injection
- Register services in extension methods: `RegisterApplicationServices()`, `RegisterInfrastructureServices()`
- Use constructor injection
- Prefer primary constructors in C# 12+ (e.g., `public class Handler(IService service)`)

#### Error Handling
- Use `Result<T>` pattern from Shared for operations that can fail
- Define custom error types inheriting from `Error` base class
- Return errors in mutations using `[Error<TError>]` attributes
- Log errors appropriately (info for business errors, error for exceptions)

---

### Frontend (Next.js/TypeScript)

#### Project Structure
- **app/[locale]**: App Router pages with i18n support
- **components/common**: Reusable UI components (buttons, forms, layouts)
- **components/sections**: Page-specific sections
- **graphql**: GraphQL queries/mutations organized by domain
- **generated**: Auto-generated TypeScript types from GraphQL schema
- **lib**: Utilities, Apollo client setup, routes
- **hooks**: Custom React hooks
- **contexts**: React Context providers
- **types**: TypeScript type definitions

#### Naming Conventions
- **Components**: PascalCase (e.g., `ProductCard.tsx`)
- **Hooks**: camelCase with "use" prefix (e.g., `useProductFilters.ts`)
- **GraphQL Operations**: UPPER_SNAKE_CASE (e.g., `GET_PRODUCTS_QUERY`)
- **Types/Interfaces**: PascalCase (e.g., `ProductFilterProps`)
- **File names**: kebab-case for non-component files (e.g., `apollo-client.ts`)

#### GraphQL with Apollo Client
- Define queries/mutations in `src/graphql/{domain}/{operation}.ts`
- Use GraphQL Code Generator to generate TypeScript types and hooks
- Generated files use `.generated.tsx` suffix
- Use generated hooks (`useGetProductsQuery`, `useCreateProductMutation`)
- Configure Apollo Client with auth headers from NextAuth session

```typescript
// src/graphql/products/getProducts.ts
import { gql } from '@apollo/client';

export const GET_PRODUCTS_QUERY = gql`
    query GetProducts($categoryId: ID) {
        products(where: { categoryId: { eq: $categoryId } }) {
            id
            name
            description
        }
    }
`;

// Usage (in component)
const { data, loading, error } = useGetProductsQuery({
    variables: { categoryId: selectedCategory },
});
```

#### Component Patterns
- Use **Server Components** by default for data fetching
- Use **Client Components** (`'use client'`) for interactivity, hooks, context
- Prefer async Server Components for initial data loading
- Use Apollo Client's `getClient()` for server-side queries
- Use Suspense boundaries for loading states

```typescript
// Server Component Example
import { getClient } from '@/lib/ApolloClient';
import { GET_PRODUCTS_QUERY } from '@/graphql/products/getProducts';

export default async function ProductsPage() {
    const { data } = await getClient().query({
        query: GET_PRODUCTS_QUERY,
    });
    
    return <ProductList products={data.products} />;
}
```

#### Internationalization (i18n)
- Use `next-intl` for all user-facing strings
- Define translations in `messages/{locale}/*.json`
- Use `useTranslations()` hook in Client Components
- Use `getTranslations()` in Server Components
- Organize translations by domain (auth, common, dashboard, etc.)

```typescript
// Client Component
import { useTranslations } from 'next-intl';

export default function LoginForm() {
    const t = useTranslations('auth');
    return <button>{t('login')}</button>;
}

// Server Component
import { getTranslations } from 'next-intl/server';

export default async function Page() {
    const t = await getTranslations('common');
    return <h1>{t('welcome')}</h1>;
}
```

#### Authentication
- Use NextAuth.js v5 for authentication
- Access session via `auth()` in Server Components
- Access session via `useSession()` in Client Components
- Include Bearer token in Apollo Client requests
- Protect routes using middleware.ts

#### Forms & Validation
- Use React Hook Form for form management
- Use Zod for schema validation
- Integrate with `@hookform/resolvers/zod`
- Use custom form components from `components/common/ui`

```typescript
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { z } from 'zod';

const schema = z.object({
    email: z.string().email(),
    password: z.string().min(8),
});

const form = useForm({
    resolver: zodResolver(schema),
});
```

#### Styling
- Use Tailwind CSS for all styling
- Follow utility-first approach
- Use `cn()` helper (from `lib/utils.ts`) for conditional classes
- Prefer component composition over complex styling
- Use CSS variables for theme values
- Support dark mode via `next-themes`

#### State Management
- Use React Context for global app state
- Use `nuqs` for URL-based filter/search state
- Use Apollo Client cache for GraphQL data
- Avoid prop drilling - use Context or composition

#### TypeScript Best Practices
- Enable strict mode
- Use explicit return types for public APIs
- Prefer interfaces for object shapes, types for unions/primitives
- Use generics for reusable components
- Avoid `any` - use `unknown` if type is truly unknown

---

## Development Workflows

### Backend Development

#### Adding a New Microservice
1. Create folder structure: Domain, Application, Infrastructure, API, Shared
2. Add project references in `.sln` and `docker-compose.yml`
3. Set up DbContext with EF Core migrations
4. Configure GraphQL schema and types
5. **Run Fusion generation script** after schema changes: `generate_pack_compose_fusion.ps1`
6. Update `.env` file with connection strings

#### Adding a New Feature (CQRS)
1. Define command/query in `Application/Features/{Entity}/{Commands|Queries}/{FeatureName}/`
2. Create handler implementing `IRequestHandler<TRequest, TResponse>`
3. Add validator if needed: `{CommandName}Validator`
4. Create GraphQL mutation or query in API layer
5. Test using Banana Cake Pop (GraphQL IDE)

#### Database Migrations
```powershell
# From microservice project directory
dotnet ef migrations add MigrationName --project ../Infrastructure
dotnet ef database update --project ../Infrastructure
```

#### After GraphQL Schema Changes
- Always run `generate_pack_compose_fusion.ps1` from `backend/scripts/`
- This packs subgraph schemas and composes the Fusion gateway schema
- Required for gateway to serve updated federated schema

### Frontend Development

#### After GraphQL Schema Changes
```powershell
# From frontend directory
pnpm generate  # Runs GraphQL Code Generator
```

#### Adding a New Page
1. Create route in `app/[locale]/{route}/page.tsx`
2. Add translations in `messages/{locale}/`
3. Create GraphQL queries in `src/graphql/{domain}/`
4. Run `pnpm generate` to generate TypeScript types
5. Import and use generated hooks

#### Creating UI Components
- Use Radix UI primitives as base
- Place reusable components in `components/common/ui/`
- Follow existing component patterns (Button, Input, Dialog, etc.)
- Support theming via CSS variables

---

## Important Commands

### Backend
```powershell
# Generate HTTPS certificates (first time setup)
cd backend/scripts
./generate_certificates.ps1

# Generate and compose Fusion gateway schema
cd backend/scripts
./generate_pack_compose_fusion.ps1

# Run with Docker Compose
cd backend
docker-compose up --build

# EF Core migrations (from microservice directory)
dotnet ef migrations add MigrationName --project ../Infrastructure
dotnet ef database update --project ../Infrastructure
```

### Frontend
```powershell
# Install dependencies
pnpm install

# Generate GraphQL types
pnpm generate

# Run dev server with HTTPS
pnpm dev

# Build for production
pnpm build
```

---

## Configuration Files

### Backend
- **Directory.Packages.props**: Central package version management
- **.env**: Connection strings, OpenIddict config, Elasticsearch URL
- **docker-compose.yml**: Service orchestration
- **launchSettings.json**: Local dev settings

### Frontend
- **package.json**: Dependencies and scripts
- **.env.local**: NextAuth secret, API URLs
- **next.config.ts**: Next.js configuration
- **apollo.config.ts**: Apollo tooling config
- **codegen.ts**: GraphQL Code Generator config
- **tsconfig.json**: TypeScript configuration

---

## Testing & Debugging

### Backend
- Use Banana Cake Pop for GraphQL testing (auto-started with HotChocolate)
- Access at: `http://localhost:{port}/graphql` for each microservice
- Gateway typically at: `http://localhost:8080/graphql`
- Check RabbitMQ Management: `http://localhost:15672` (guest/guest)

### Frontend
- Use React DevTools for component inspection
- Use Apollo DevTools extension for GraphQL debugging
- Check Network tab for GraphQL requests
- Use `console.log` sparingly - prefer React DevTools

---

## Common Pitfalls & Best Practices

### Backend
- ✅ **Always** run Fusion script after GraphQL schema changes
- ✅ Use primary constructors for cleaner dependency injection
- ✅ Await domain event handlers properly in `SaveChangesAsync`
- ✅ Use cancellation tokens in all async operations
- ✅ Index Elasticsearch on entity create/update via domain events
- ❌ Don't access DbContext directly in API layer - use MediatR
- ❌ Don't forget to register services in DI container
- ❌ Don't mix CQRS responsibilities (commands shouldn't return large data)

### Frontend
- ✅ **Always** run `pnpm generate` after backend schema changes
- ✅ Use Server Components for initial data fetching
- ✅ Use translation keys for all user-facing text
- ✅ Handle loading and error states in Apollo queries
- ✅ Use semantic HTML and accessibility attributes
- ❌ Don't fetch data in Client Components unless needed for interactivity
- ❌ Don't hardcode strings - use i18n translations
- ❌ Don't forget to await `params` in page components (Next.js 15 requirement)

---

## When Generating Code

### For Backend Features
1. Identify the microservice (Products, Inventory, Price, User)
2. Follow Clean Architecture layers
3. Create command/query with handler
4. Add FluentValidation validator
5. Create GraphQL mutation/query in API layer
6. Remind to run Fusion generation script
7. Use existing Shared abstractions (Result<T>, IElasticsearchService, etc.)

### For Frontend Features
1. Create GraphQL query/mutation in appropriate domain folder
2. Remind to run `pnpm generate`
3. Create Server Component for page/section
4. Use generated Apollo hooks
5. Add i18n translations
6. Follow existing component patterns
7. Handle loading/error states

---

## File Templates

### Backend Command Handler
```csharp
namespace {Microservice}.Application.Features.{Entity}.Commands.{FeatureName};

public record {Feature}Command(...) : IResultCommand<{ReturnType}>;

public class {Feature}CommandValidator : AbstractValidator<{Feature}Command>
{
    public {Feature}CommandValidator()
    {
        // Validation rules
    }
}

public class {Feature}CommandHandler(I{Microservice}DbContext dbContext) 
    : IRequestHandler<{Feature}Command, Result<{ReturnType}>>
{
    public async Task<Result<{ReturnType}>> Handle(
        {Feature}Command request, 
        CancellationToken cancellationToken)
    {
        // Implementation
    }
}
```

### Frontend GraphQL Query
```typescript
// src/graphql/{domain}/{operation}.ts
import { gql } from '@apollo/client';

export const {OPERATION_NAME}_QUERY = gql`
    query {OperationName}($param: Type) {
        {resource}(where: { field: { eq: $param } }) {
            id
            field1
            field2
        }
    }
`;
```

---

## Quick Reference

### Backend Ports
- Gateway: 8080
- Auth Server: 8085
- Products API: 8081
- Inventory API: 8082
- Price API: 8083
- User API: 8084

### Frontend Port
- Development: 3000 (HTTPS)

### Infrastructure
- RabbitMQ: 5672 (AMQP), 15672 (Management UI)
- SQL Server: 1433
- Elasticsearch: 9200

---

## Additional Notes

- This project uses **HotChocolate Fusion** (distributed GraphQL architecture) - different from Apollo Federation
- **Outbox pattern** is implemented for reliable messaging
- **Domain events** trigger side effects (Elasticsearch indexing, message publishing)
- Frontend uses **App Router** (not Pages Router)
- All microservices share a common **Shared** project for cross-cutting concerns
- Central package management ensures version consistency across microservices
