# EcommerceAPI — Project Context for AI Agents

## Project Overview
A full-stack E-commerce platform built as a portfolio project to learn .NET.
- **Backend:** ASP.NET Core 8 REST API
- **Frontend:** Angular (to be added after API is complete)
- **Hosting:** Railway

## Architecture
**Clean Architecture** with 4 layers:
```
EcommerceAPI/
└── src/
    ├── EcommerceAPI.Domain/          # Entities, interfaces, domain rules (no dependencies)
    ├── EcommerceAPI.Application/     # Use cases, CQRS handlers, DTOs, validators
    ├── EcommerceAPI.Infrastructure/  # EF Core, PostgreSQL, Redis, Stripe, Hangfire
    └── EcommerceAPI.API/             # Controllers, middleware, Swagger, startup config
```

Dependency direction: `API → Infrastructure → Application → Domain`

## Tech Stack

### Backend
| Technology | Purpose |
|---|---|
| ASP.NET Core 8 | Web API framework |
| Entity Framework Core | ORM for PostgreSQL |
| PostgreSQL | Primary database |
| MediatR | CQRS pattern (commands & queries) |
| FluentValidation | Input validation |
| AutoMapper | DTO mapping |
| ASP.NET Identity | User management |
| JWT + Refresh Tokens | Authentication |
| Redis | Caching (products, categories) |
| Hangfire | Background jobs (emails, cart cleanup) |
| Stripe SDK | Payment processing |
| Swagger / OpenAPI | API documentation |
| Docker | Containerization |
| xUnit + Moq | Testing |

### Frontend (planned)
| Technology | Purpose |
|---|---|
| Angular | Frontend SPA framework |
| TypeScript | Language |
| Node.js / npm | Runtime and package manager |

## Modules
| Module | Description |
|---|---|
| Auth | Register, Login, Refresh Token, Roles (Admin/Customer) |
| Products | CRUD, categories, images, stock management |
| Cart | Add/remove products, calculate totals |
| Orders | Create order, history, status (Pending/Paid/Shipped/Cancelled) |
| Payments | Stripe integration, webhooks |
| Inventory | Auto-update on purchase |
| Cache | Redis for products and categories |
| Background Jobs | Hangfire for emails, abandoned cart cleanup |

## CQRS Convention
- Commands → mutate state (Create, Update, Delete)
- Queries → read state (Get, List, Search)
- Each feature folder contains: `Command/Query`, `Handler`, `Validator`, `DTO`

## Key Decisions
- **PostgreSQL over SQL Server** — free to host, production-grade
- **Repository Pattern** — used over raw EF Core for testability
- **JWT + ASP.NET Identity** — full user management with role-based auth
- **Angular frontend** — added after API is complete, common enterprise stack with .NET
- **Railway** — free tier hosting for both API and PostgreSQL

## Developer Notes
- User is learning .NET for the first time — explain concepts when introducing new ones
- Keep explanations step-by-step, one module at a time
- Frontend (Angular) is a future phase, focus on API first
