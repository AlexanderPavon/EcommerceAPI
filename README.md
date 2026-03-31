# EcommerceAPI

A production-ready RESTful API for an e-commerce platform built with ASP.NET Core 10 and Clean Architecture.

## Architecture

This project follows **Clean Architecture** with a strict dependency rule — outer layers depend on inner layers, never the other way around.

```
EcommerceAPI/
└── src/
    ├── EcommerceAPI.Domain/          # Entities, enums, domain rules. No external dependencies.
    ├── EcommerceAPI.Application/     # Use cases, CQRS handlers, DTOs, interfaces.
    ├── EcommerceAPI.Infrastructure/  # EF Core, PostgreSQL, Redis, Stripe, Hangfire implementations.
    └── EcommerceAPI.API/             # Controllers, middleware, dependency injection entry point.
```

Dependency direction: `API → Infrastructure → Application → Domain`

## Tech Stack

| Layer | Technology | Purpose |
|---|---|---|
| API | ASP.NET Core 10 | Web framework |
| ORM | Entity Framework Core | Database access |
| Database | PostgreSQL | Primary data store |
| Cache | Redis | Product/category caching |
| Auth | ASP.NET Identity + JWT | Authentication & authorization |
| Messaging | MediatR | CQRS pattern |
| Validation | FluentValidation | Request validation |
| Payments | Stripe | Payment processing |
| Jobs | Hangfire | Background jobs & scheduling |
| Containers | Docker | Containerization |

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Docker](https://www.docker.com/)
- [PostgreSQL](https://www.postgresql.org/)

## Getting Started

### 1. Clone the repository

```bash
git clone https://github.com/AlexanderPavon/EcommerceAPI.git
cd EcommerceAPI
```

### 2. Configure environment variables

Create `src/EcommerceAPI.API/appsettings.json` (not tracked by git):

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=EcommerceDB;Username=postgres;Password=YOUR_PASSWORD",
    "Redis": "localhost:6379"
  },
  "Jwt": {
    "Key": "your-secret-key-minimum-32-characters",
    "Issuer": "EcommerceAPI",
    "Audience": "EcommerceClient"
  },
  "Stripe": {
    "SecretKey": "sk_test_YOUR_STRIPE_KEY"
  }
}
```

### 3. Start Redis with Docker

```bash
docker run -d --name redis-ecommerce -p 6379:6379 redis:alpine
```

### 4. Apply database migrations

```bash
dotnet ef database update --project src/EcommerceAPI.Infrastructure --startup-project src/EcommerceAPI.API
```

### 5. Run the API

```bash
dotnet run --project src/EcommerceAPI.API
```

API will be available at `http://localhost:5081`
Hangfire dashboard at `http://localhost:5081/hangfire`
OpenAPI spec at `http://localhost:5081/openapi/v1.json`

## API Endpoints

### Auth
| Method | Endpoint | Access | Description |
|---|---|---|---|
| POST | `/api/auth/register` | Public | Register a new user |
| POST | `/api/auth/login` | Public | Login and receive JWT |

### Categories
| Method | Endpoint | Access | Description |
|---|---|---|---|
| GET | `/api/categories` | Public | List all categories |
| POST | `/api/categories` | Admin | Create a category |

### Products
| Method | Endpoint | Access | Description |
|---|---|---|---|
| GET | `/api/products` | Public | List products (supports `?search=` and `?categoryId=`) |
| GET | `/api/products/{id}` | Public | Get product by ID |
| POST | `/api/products` | Admin | Create a product |
| PUT | `/api/products/{id}` | Admin | Update a product |
| DELETE | `/api/products/{id}` | Admin | Soft delete a product |

### Cart
| Method | Endpoint | Access | Description |
|---|---|---|---|
| GET | `/api/cart` | Authenticated | Get current user's cart |
| POST | `/api/cart/items` | Authenticated | Add item to cart |
| DELETE | `/api/cart/items/{cartItemId}` | Authenticated | Remove item from cart |

### Orders
| Method | Endpoint | Access | Description |
|---|---|---|---|
| GET | `/api/orders` | Authenticated | List current user's orders |
| GET | `/api/orders/{id}` | Authenticated | Get order details |
| POST | `/api/orders` | Authenticated | Create order from cart |
| PUT | `/api/orders/{id}/status` | Admin | Update order status |

### Payments
| Method | Endpoint | Access | Description |
|---|---|---|---|
| POST | `/api/payments/create-intent/{orderId}` | Authenticated | Create Stripe payment intent |
| POST | `/api/payments/webhook` | Public | Stripe webhook handler |

## Running with Docker

### Build the image

```bash
docker build -t ecommerce-api .
```

### Run the container

```bash
docker run -p 8080:8080 \
  -e ConnectionStrings__DefaultConnection="Host=host.docker.internal;Port=5432;Database=EcommerceDB;Username=postgres;Password=YOUR_PASSWORD" \
  -e ConnectionStrings__Redis="host.docker.internal:6379" \
  -e Jwt__Key="your-secret-key-minimum-32-characters" \
  -e Jwt__Issuer="EcommerceAPI" \
  -e Jwt__Audience="EcommerceClient" \
  -e Stripe__SecretKey="sk_test_YOUR_KEY" \
  ecommerce-api
```

## Background Jobs

Hangfire runs the following recurring jobs:

| Job | Schedule | Description |
|---|---|---|
| `cleanup-abandoned-carts` | Daily (midnight) | Removes carts older than 7 days |

## Project Structure

```
src/
├── EcommerceAPI.Domain/
│   ├── Entities/          # BaseEntity, Product, Category, Order, OrderItem, Cart, CartItem
│   └── Enums/             # OrderStatus
├── EcommerceAPI.Application/
│   ├── Common/
│   │   └── Interfaces/    # IApplicationDbContext, IJwtService, IPaymentService, ICacheService
│   └── Features/
│       ├── Auth/          # Register, Login
│       ├── Products/      # CreateProduct, GetProducts, GetProductById, UpdateProduct, DeleteProduct
│       ├── Categories/    # CreateCategory, GetCategories
│       ├── Cart/          # GetCart, AddToCart, RemoveFromCart
│       ├── Orders/        # CreateOrder, GetOrders, GetOrderById, UpdateOrderStatus
│       └── Payments/      # CreatePaymentIntent
├── EcommerceAPI.Infrastructure/
│   ├── Persistence/       # ApplicationDbContext, EF Core configurations
│   └── Services/          # JwtService, StripePaymentService, RedisCacheService, CartCleanupJob
└── EcommerceAPI.API/
    └── Controllers/       # AuthController, ProductsController, CategoriesController, CartController, OrdersController, PaymentsController
```
