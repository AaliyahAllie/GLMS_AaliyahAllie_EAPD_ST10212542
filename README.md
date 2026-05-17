# Global Logistics Management System (GLMS)

Enterprise logistics management platform built using **ASP.NET Core MVC**, **SQL Server**, and **Entity Framework Core** for TechMove Logistics.

---

# Project Overview

TechMove Logistics previously relied on spreadsheets, emails, and manual processes to manage freight contracts and service requests. This resulted in workflow bottlenecks, lost invoices, compliance issues, and fragmented data management.

The **Global Logistics Management System (GLMS)** was developed to centralize logistics operations into a single enterprise web platform.

The system includes:

* Client management
* Contract management
* Service request management
* Currency conversion
* PDF agreement uploads
* Workflow validation
* Public customer website
* Secure admin portal
* Unit testing

---

# Technologies Used

| Component       | Technology                         |
| --------------- | ---------------------------------- |
| Frontend        | ASP.NET Core MVC + Bootstrap       |
| Backend         | C# ASP.NET Core                    |
| Database        | SQL Server                         |
| ORM             | Entity Framework Core              |
| Authentication  | ASP.NET Core Cookie Authentication |
| API Integration | HttpClient                         |
| Testing         | xUnit                              |

---

# System Architecture

The project follows the **Model-View-Controller (MVC)** architecture pattern and uses a **Monolithic Architecture** where frontend, backend logic, and database access are contained within a single deployable application.

## MVC Components

### Models

* Client
* Contract
* ContractStatus
* ServiceRequest
* ServiceRequestStatus
* ServiceInquiry
* LoginViewModel

### Controllers

* HomeController
* ClientsController
* ContractsController
* ServiceRequestsController
* PublicController

### Views

Razor Views and Bootstrap were used to create a responsive enterprise user interface.

---

# Public Website

The public-facing website allows customers to:

* View company information
* Browse logistics services
* Learn about TechMove Logistics
* Submit logistics service inquiries

## Public Pages

* Home
* About Us
* Services
* Contact Us

---

# Admin Portal

The administrator portal is protected using **ASP.NET Core Cookie Authentication**.

Only authenticated administrators can access:

* Client Management
* Contract Management
* Service Requests
* Dashboard Analytics
* PDF Agreement Uploads
* Workflow Validation

---

# Features

## Client Management

* Create clients
* Edit client information
* Delete clients
* View client details

---

## Contract Management

* Create contracts
* Assign contracts to clients
* Manage contract statuses
* Upload signed agreements
* Download signed agreements
* Search/filter contracts

---

## Service Requests

* Create service requests
* Link requests to contracts
* Automatically convert USD to ZAR
* View service request details

---

## Workflow Validation

Workflow validation prevents service requests from being created for contracts with the following statuses:

| Contract Status | Service Request Allowed |
| --------------- | ----------------------- |
| Draft           | Yes                     |
| Active          | Yes                     |
| Expired         | No                      |
| OnHold          | No                      |

Validation was implemented using the `ContractWorkflowService`.

---

## External API Integration

The system integrates with a currency exchange API using `HttpClient` to retrieve live USD-to-ZAR exchange rates.

Features include:

* Automatic currency conversion
* Live exchange rate retrieval
* Fallback exchange rate handling

---

## File Handling

The system supports PDF agreement uploads and downloads.

Validation includes:

* PDF-only uploads
* MIME type validation
* File extension validation
* Empty file prevention

Uploaded files are stored inside:

```text
wwwroot/uploads/contracts
```

---

## Search and Filtering

LINQ-based filtering allows administrators to:

* Search by contract status
* Filter by start date
* Filter by end date

---

## Async/Await Implementation

Asynchronous programming was implemented using `async` and `await` for:

* Database queries
* File uploads
* API requests
* Data saving

---

# Database Design

## Main Tables

* Clients
* Contracts
* ServiceRequests
* ServiceInquiries

## SQL Server

The project uses SQL Server together with Entity Framework Core.

Database migrations were implemented using EF Core migrations.

---

# Unit Testing

Unit tests were implemented using **xUnit**.

## Tested Components

* Contract Workflow Validation
* Currency Conversion Logic
* File Validation Service

---

# Project Structure

```text
GLMS.Web
│
├── Controllers
├── Models
├── Views
├── Data
├── Services
├── Migrations
├── wwwroot
│   ├── css
│   ├── images
│   └── uploads
```

---

# Setup Instructions

## Clone Repository

```bash
git clone https://github.com/AaliyahAllie/GLMS_AaliyahAllie_EAPD_ST10212542.git
```

---

## Configure Database

Update `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOURSERVER;Database=GLMS_DB;Trusted_Connection=True;TrustServerCertificate=True"
  }
}
```

---

## Run Database Migrations

```bash
dotnet ef database update
```

---

## Run Application

```bash
dotnet run
```

---

# Admin Login

```text
Username: admin
Password: Admin@123
```

---

# Links

| Platform     | Link                                                                                                                   |
| ------------ | ---------------------------------------------------------------------------------------------------------------------- |
| GitHub       | [GLMS GitHub Repository](https://github.com/AaliyahAllie/GLMS_AaliyahAllie_EAPD_ST10212542.git?utm_source=chatgpt.com) |
| YouTube Demo | [GLMS System Demonstration](https://youtu.be/qcLyIenQOZA?utm_source=chatgpt.com)                                       |

---

# Conclusion

The Global Logistics Management System successfully replaced manual logistics management processes with a centralized enterprise solution for TechMove Logistics. The system implemented ASP.NET Core MVC, SQL Server integration, Entity Framework Core, workflow validation, external API integration, file handling, async programming, and automated unit testing. Overall, the project improves operational efficiency, contract compliance, customer communication, and centralized data management while providing a scalable foundation for future enterprise development.

