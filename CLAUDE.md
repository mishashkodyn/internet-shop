# Project Context

Two-layer application:
- **Shop**: e-commerce module (products, cart, orders, payments)
- **Blog**: posts, comments, categories, tags

The two modules interact: blog posts can reference products, products can show related posts, shared user/auth system.

## Stack
- Backend: .NET 10 Web API, EF Core, Clean Architecture (Domain / Application / Infrastructure / API)
- Frontend: Angular 21+, standalone components, signals where appropriate, RxJS for streams
- Database: MSSQL, EF Core Code-First migrations

## Folder structure
- /backend/src/Shop.* — shop bounded context
- /backend/src/Blog.* — blog bounded context
- /backend/src/Shared.* — shared kernel (User, Auth, common types)
- /frontend/src/app/shop — shop feature module
- /frontend/src/app/blog — blog feature module
- /frontend/src/app/shared — shared services, guards, interceptors

## Conventions
- DTOs in Application layer, never expose EF entities to API
- Angular: feature modules lazy-loaded, services with providedIn: 'root' for singletons
- Migrations: one per logical change, descriptive names