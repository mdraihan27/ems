# Employee Management System

A full-stack Employee Management System built with **.NET 10** (Clean Architecture + CQRS) and **React 18** (TypeScript + Tailwind CSS).

## Tech Stack

| Layer | Technology |
|-------|-----------|
| **Backend** | .NET 10, ASP.NET Core Web API, C# 13 |
| **Frontend** | React 18, TypeScript, Tailwind CSS |
| **Database** | PostgreSQL 17 + EF Core 10 |
| **Auth** | ASP.NET Identity + JWT (Access + Refresh Tokens) |
| **Validation** | FluentValidation (backend) + Zod (frontend) |
| **PDF Reports** | QuestPDF |
| **Architecture** | Clean Architecture, CQRS (MediatR), Repository Pattern |

## Features

- **Employee CRUD** — Name, NID (10/17 digits), Phone (BD format), Department, Basic Salary
- **Family Management** — Spouse (1:1) and Children (1:Many) with cascade operations
- **Role-Based Access** — Admin (full CRUD) and Viewer (read-only)
- **Search** — Single input, case-insensitive search across Name, NID, Department, Salary with 400ms debounce
- **PDF Export** — Filtered employee list table report and individual CV with family details
- **Dark/Light Theme** — Tailwind dark mode with custom brand palette
- **Concurrency Control** — EF Core RowVersion for optimistic concurrency

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [Node.js 18+](https://nodejs.org/) and npm
- [PostgreSQL 15+](https://www.postgresql.org/download/)

## Database Setup

1. Install and start PostgreSQL.

2. (Optional) Create a database (or let EF Core create it automatically on first migration):

   ```sql
   CREATE DATABASE "EmployeeManagementDb";
   ```

3. Verify the connection string in `backend/src/EmployeeManagement.API/appsettings.Development.json`:

   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Host=localhost;Port=5432;Database=EmployeeManagementDb;Username=postgres;Password=postgresql"
   }
   ```

   Update the `Username` and `Password` to match your PostgreSQL credentials.

## Running the Backend

### 1. Trust the HTTPS development certificate

The API runs on HTTPS. Trust the .NET dev certificate once so your browser and frontend don't get SSL errors:

```bash
dotnet dev-certs https --trust
```

> **Windows users — you will see two prompts:**
> 1. **UAC elevation prompt** — click **Yes** to allow the command to run as administrator.
> 2. **Windows Security dialog** — *"Do you want to install this certificate?"* — click **Yes** to add the certificate to your Trusted Root Certification Authorities store.
>
> Both prompts must be accepted for the certificate to be trusted. This only needs to be done once per machine.

### 2. Start the API

```bash
cd backend/src/EmployeeManagement.API

# Restore dependencies
dotnet restore

# Apply EF Core migrations (creates the database schema)
dotnet ef database update --project ../EmployeeManagement.Infrastructure
# Note: You will see a "HostAbortedException: The host was aborted" message in the output.
# This is EXPECTED and NORMAL — dotnet ef intentionally aborts the host after reading
# the DbContext configuration. The migration is successful if the output ends with "Done."

# Run the API (https://localhost:5001)
dotnet run --urls "https://localhost:5001"
```

On first startup, the database is automatically seeded with:
- **Admin account** — `admin@ems.com` / `Admin@123`
- **Viewer account** — `viewer@ems.com` / `Viewer@123`
- **10 sample employees** with realistic Bangladeshi names, NIDs, and family data

## Running the Frontend

### 1. Configure the environment

The frontend reads the backend URL from a `.env` file. Copy the example and adjust if your backend runs on a different port:

```bash
cd frontend
cp .env.example .env   # Windows: copy .env.example .env
```

Add `.env` contents (edit as needed):

```env
REACT_APP_API_BASE_URL=https://localhost:5001/api
```

> **Important:** The `.env` file is git-ignored. Never commit real secrets to source control. Use `.env.example` as the template.

### 2. Install dependencies and start

```bash
# Install dependencies
# --legacy-peer-deps is required because react-scripts@5 declares a peer dependency
# on TypeScript ^3||^4, but this project uses TypeScript 5. The flag bypasses that
# outdated constraint — the code compiles and runs correctly with TypeScript 5.
npm install --legacy-peer-deps

# Start the dev server (http://localhost:3000)
npm start
```

## Project Structure

```
├── backend/
│   ├── docs/                          # SRS & API documentation
│   │   ├── SRS_Document.md
│   │   └── API_Documentation.md
│   └── src/
│       ├── EmployeeManagement.API/          # Controllers, Middleware, Program.cs
│       ├── EmployeeManagement.Application/  # DTOs, CQRS Handlers, Validators
│       ├── EmployeeManagement.Domain/       # Entities (Employee, Spouse, Child)
│       ├── EmployeeManagement.Infrastructure/ # EF Core, Repositories, Identity
│       └── EmployeeManagement.Shared/       # ApiResponse, Exceptions
└── frontend/
    └── src/
        ├── components/    # Reusable UI components
        ├── hooks/         # useAuth, useEmployees, useDebounce, useTheme
        ├── pages/         # Dashboard, Employee CRUD, Reports, Auth
        ├── services/      # Axios API clients
        └── types/         # TypeScript interfaces
```

## API Endpoints

| Method | Endpoint | Auth | Description |
|--------|---------|------|-------------|
| POST | `/api/auth/register` | — | Register a new user |
| POST | `/api/auth/login` | — | Login and receive JWT |
| POST | `/api/auth/refresh-token` | — | Refresh access token |
| GET | `/api/employees` | Viewer+ | List/search employees (paginated) |
| GET | `/api/employees/{id}` | Viewer+ | Get employee with family |
| POST | `/api/employees` | Admin | Create employee |
| PUT | `/api/employees/{id}` | Admin | Update employee |
| DELETE | `/api/employees/{id}` | Admin | Delete employee |
| POST | `/api/employees/{id}/spouse` | Admin | Add spouse |
| PUT | `/api/employees/{id}/spouse` | Admin | Update spouse |
| DELETE | `/api/employees/{id}/spouse` | Admin | Remove spouse |
| POST | `/api/employees/{id}/children` | Admin | Add child |
| PUT | `/api/employees/{id}/children/{childId}` | Admin | Update child |
| DELETE | `/api/employees/{id}/children/{childId}` | Admin | Remove child |
| GET | `/api/employees/reports/list` | Viewer+ | Download employee list PDF |
| GET | `/api/employees/{id}/reports/cv` | Viewer+ | Download employee CV PDF |
