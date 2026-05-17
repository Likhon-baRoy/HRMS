# HRMS

Minimal HRMS assessment project built with ASP.NET 9, Entity Framework Core, PostgreSQL, and Angular 21.

## Modules

- Employee Management
- Salary Management
- Payroll Management
- Authentication and authorization

## Architecture

- `API/` contains the ASP.NET Core Web API.
- `client/` contains the Angular 21 frontend.
- EF Core handles persistence against PostgreSQL.
- JWT is used for authentication.
- FluentValidation handles request validation.
- AutoMapper maps entities to DTOs.

## Backend setup

1. Configure the connection string in `API/appsettings.json`.
2. Run the API:

```bash
cd API
dotnet run
```

The API listens on:
- `http://localhost:5000`
- `https://localhost:5001`

## Frontend setup

1. Update `client/src/environments/environment.ts` if needed.
2. Run the Angular app:

```bash
cd client
npm install
npm run start
```

The frontend runs on:
- `http://localhost:4200`

## Seeded login

The database seeder creates these accounts:

- Admin: `admin` / `Admin@123`
- HR: `hr` / `Hr@123`
- Manager: `manager` / `Manager@123`
- Employee: `employee` / `Employee@123`

## Database schema summary

Main tables:

- `employees`
- `user_accounts`
- `departments`
- `positions`
- `attendances`
- `salaries`
- `SalaryRevisions`
- `payrolls`
- `bonuses`
- `deductions`
- `PayrollDeductions`

Important relationships:

- Each employee belongs to one department and one position.
- Each employee can have one current salary and salary revision history.
- Payroll rows belong to employees and are generated per pay period.

## Current minimal implementation

- Employee CRUD with basic search and filters
- Salary assign/update with current salary rule
- Monthly payroll generation for all active employees
- Payroll list page
- JWT login and route protection
