# Purchase Bill Management System

A Full Stack Purchase Bill Management application developed as a technical assignment using **Angular**, **ASP.NET Core Web API (.NET 8)**, **Entity Framework Core**, and **SQL Server**.

---

## Technology Stack

### Frontend
- Angular (Latest)
- TypeScript
- Angular Material
- Reactive Forms
- RxJS

### Backend
- ASP.NET Core Web API (.NET 8)
- Entity Framework Core
- SQL Server
- REST API
- JWT Authentication
- Swagger

### Database
- SQL Server
- Entity Framework Core Migrations

---

# Features

## Authentication

- User Login
- External API Authentication
- JWT Authentication
- Protected Routes
- Session Management

---

## Purchase Bill

- Add Purchase Bill
- Item Autocomplete
- Batch Selection
- Real-time Calculations
- Dynamic Item Table
- Item Summary
- Form Validation

---

## Item Calculations

Example

| Field | Value |
|-------|-------|
| Standard Cost | 100 |
| Standard Price | 150 |
| Quantity | 5 |
| Discount | 20% |

Calculation

```
Cost Before Discount = 100 × 5 = 500

Discount = 20% of 500 = 100

Total Cost = 500 - 100 = 400

Total Selling = 150 × 5 = 750
```

---

# Project Structure

```
purchase-bill-management
│
├── backend
│   └── PurchaseBill.Api
│
├── frontend
│   └── purchase-bill-ui
│
├── database
│   └── PurchaseBill.sql
│
└── README.md
```

---

# Backend Architecture

```
PurchaseBill.Api
│
├── Controllers
├── Data
├── DTOs
├── Entities
├── Interfaces
├── Repositories
├── Services
├── Helpers
├── Middleware
├── Migrations
├── Program.cs
└── appsettings.json
```

---

# Database Tables

- Location_Details
- Purchase_Bills
- Purchase_Bill_Items

---

# External Login API

```
POST
https://ez-staging-api.azurewebsites.net/api/External_Api/POS_Api/Invoke
```

Request

```json
{
  "API_Action": "GetLoginData",
  "Device_Id": "D001",
  "Sync_Time": "",
  "Company_Code": "<email>",
  "API_Body": {
    "Username": "<email>",
    "Pw": "<password>"
  }
}
```

---

# Setup Instructions

## Clone Repository

```bash
git clone https://github.com/<your-username>/purchase-bill-management.git
```

---

## Backend

Navigate to backend

```bash
cd backend/PurchaseBill.Api
```

Restore packages

```bash
dotnet restore
```

Run migrations

```bash
dotnet ef database update
```

Run API

```bash
dotnet run
```

Swagger

```
https://localhost:xxxx/swagger
```

---

## Frontend

Navigate

```bash
cd frontend/purchase-bill-ui
```

Install packages

```bash
npm install
```

Run Angular

```bash
ng serve
```

Application

```
http://localhost:4200
```

---

# Configuration

Update the SQL Server connection string inside

```
appsettings.json
```

Example

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=PurchaseBillDb;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

---

# Screens

- Login
- Purchase Bill
- Item Table
- Item Summary

---

# Validation

- Required Fields
- Email Validation
- Numeric Validation
- Quantity Validation
- Discount Validation

---

# Error Handling

- Invalid Login
- API Errors
- Validation Errors
- Database Errors

---

# API Documentation

Swagger is enabled.

```
https://localhost:xxxx/swagger
```

---

# Submission

Repository includes

- Angular Frontend
- ASP.NET Core Web API
- SQL Server Script
- Entity Framework Core Migrations
- README
- Clean Project Structure

---

# Author

**Induranga Bandara**

Full Stack Software Engineer

Email: neo.induranga@gmail.com

GitHub: https://github.com/neobros/purchase-bill-management