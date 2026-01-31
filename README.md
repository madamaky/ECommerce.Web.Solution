# ECommerce.Web.Solution 🚀

**ECommerce** is a modular ASP.NET Core solution that implements an e-commerce backend, an admin dashboard, and layered services. This repository contains projects for domain, persistence, services, presentation and a Web API that includes authentication (JWT), caching (Redis), payments (Stripe) and EF Core migrations/seeding.

---

## 🔎 Overview

- Language: C# (.NET)
- Primary framework: **ASP.NET Core**
- Target frameworks in this solution:
  - `ECommerce.Web`, `ECommerce.Persistence`, `ECommerce.Service`, etc. target **.NET 8.0**
  - `Admin.Dashboard` targets **.NET 9.0** (check project references before changing SDK)
- Key features: JWT auth, EF Core (SQL Server), Redis cache, Stripe payments, automatic DB migrations and seeding on startup, Swagger in Development

---

## 📁 Project Structure

- `ECommerce.Web/` - Main Web API and composition root (Startup/Program)
- `ECommerce.Persistence/` - EF Core DbContexts, repositories, migrations
- `ECommerce.Service/` - Business logic and services (Authentication, Basket, Orders, Payments)
- `ECommerce.Domain/` - Domain entities and contracts
- `ECommerce.Persentation/` - DTOs, controllers and presentation artifacts
- `Admin.Dashboard/` - MVC admin dashboard referencing the API

---

## ⚙️ Prerequisites

- .NET SDK 8.0+ (and .NET 9 if you build `Admin.Dashboard` locally)
- SQL Server (local or remote)
- Redis (for caching) - default dev value is `localhost`
- Optional: Stripe API key for payment flows
- `dotnet-ef` tools for creating migrations (optional for local migration management):
  - Install: `dotnet tool install --global dotnet-ef` (if not installed)

---

## 🛠 Setup & Run (Development)

1. Restore and build:
   - `dotnet restore` (in solution root)
   - `dotnet build`

2. Update configuration as needed:
   - Copy or edit `ECommerce.Web/appsettings.Development.json` and set your:
     - `ConnectionStrings:DefaultConnection` (SQL Server)
     - `ConnectionStrings:IdentityConnection` (Identity DB)
     - `ConnectionStrings:RedisConnection` (Redis host)
     - `JWTOptions:SecretKey`, `Issuer`, `Audience`
     - `StripeSettings:SecretKey` (if testing payments)

   Example (from `appsettings.Development.json`):
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=.;Database=ECommerce;Trusted_Connection=True;TrustServerCertificate=True;",
     "IdentityConnection": "Server=.;Database=ECommerceIdentity;Trusted_Connection=True;TrustServerCertificate=True;",
     "RedisConnection": "localhost"
   }
   ```

3. Run the app (it will apply migrations and seed data automatically):
   - `dotnet run --project ECommerce.Web` (development environment will enable Swagger UI)

4. Admin Dashboard (optional):
   - `dotnet run --project Admin.Dashboard`

---

## 📦 Database & Migrations

- On startup the Web project executes migration and seeding helpers (so running the project will create/update DBs automatically).
- To manage EF Core migrations manually, use `dotnet ef` specifying project and startup project. Example:

  - Add migration for main store DB:
    `dotnet ef migrations add <Name> -p ECommerce.Persistence -s ECommerce.Web -c StoreDbContext`

  - Add migration for identity DB (example):
    `dotnet ef migrations add IdentityTablesCreate -o IdentityData/Migrations -c StoreIdentityDbContext -p ECommerce.Persistence -s ECommerce.Web`

- If you prefer the Package Manager Console in Visual Studio, set the `Default project` to `ECommerce.Persistence` and the `Startup project` to `ECommerce.Web` then run `Add-Migration` / `Update-Database` as needed.

---

## 🔒 Authentication & Secrets

- JWT settings are in `JWTOptions` in appsettings. Use a strong `SecretKey` in production.
- Stripe secret key is read from `StripeSettings:SecretKey`.
- Do not commit production secrets to source control. Use environment variables, user-secrets, or a secret store in CI/CD.

---

## 🧪 Tests

- There are no dedicated test projects included in the solution (add tests under a `tests/` folder and a test project using `xUnit` or `NUnit`).

---

## 🚀 Deployment Notes

- Configure connection strings and secrets via environment variables in production.
- Ensure Redis and SQL Server are reachable by your hosting environment.
- Consider running EF migrations as part of your deployment pipeline or use built-in migration helpers at startup.
- Enable HTTPS and secure JWT secrets.

---

## 🤝 Contributing

- Fork the repo, create a feature branch, open a PR, and include clear changelog/comments.
- Add unit/integration tests for new functionality.

---

## 📜 License

- No license file included. Add a `LICENSE` file (e.g. MIT) if you want to open-source or define permitted usage.

---

If you'd like, I can also add a `CONTRIBUTING.md`, a `LICENSE`, or tailor the README to include development scripts or CI setup. ✨
