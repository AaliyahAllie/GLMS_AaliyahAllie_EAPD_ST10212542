# Global Logistics Management System (GLMS)

A centralized enterprise logistics management platform developed using **ASP.NET Core MVC**, **Entity Framework Core**, and **SQL Server**.
The system was designed for **TechMove Logistics** to replace outdated spreadsheet and email-based workflows with a modern web application for managing clients, contracts, service requests, and financial calculations. 

---

# 📌 Project Overview

The **Global Logistics Management System (GLMS)** is a monolithic enterprise web application that allows administrators to:

* Manage logistics clients
* Create and manage contracts
* Upload and download signed agreements
* Create service requests linked to contracts
* Convert USD values to ZAR using a live exchange rate API
* Search and filter contracts
* Enforce workflow validation rules
* Run automated unit tests

The application centralizes logistics operations into a single platform to improve compliance, efficiency, and data management. 

---

# 🛠️ Technologies Used

| Component       | Technology                   |
| --------------- | ---------------------------- |
| Frontend        | ASP.NET Core MVC + Bootstrap |
| Backend         | C# ASP.NET Core              |
| Database        | SQL Server                   |
| ORM             | Entity Framework Core        |
| Testing         | xUnit                        |
| API Integration | HttpClient                   |

---

# 🏗️ System Architecture

The project follows the **Model-View-Controller (MVC)** architecture pattern.

## Models

The system includes the following core models:

* Client
* Contract
* ContractStatus
* ServiceRequest
* ServiceRequestStatus

## Views

Razor Views together with Bootstrap were used to build a responsive user interface.

## Controllers

The following controllers handle application logic:

* HomeController
* ClientsController
* ContractsController
* ServiceRequestsController

The application uses a **monolithic architecture**, meaning the frontend, backend, and database access are contained within a single deployable application. 

---

# 📂 Features

## 👥 Client Management

Administrators can:

* Create clients
* Edit client information
* Delete clients
* View client details

---

## 📄 Contract Management

The system supports:

* Contract creation
* Contract assignment to clients
* Contract status management
* Uploading signed agreements
* Downloading agreements

---

## 🚚 Service Requests

Administrators can:

* Create service requests
* Link requests to contracts
* Calculate local ZAR costs
* View request details

---

# ✅ Implemented Enterprise Features

## Workflow Validation

Business rules prevent service requests from being created for contracts with an:

* Expired status
* OnHold status

This validation is handled through the `ContractWorkflowService`.

---

## 🌍 External API Integration

The system integrates with a currency exchange API using `HttpClient` to retrieve live USD-to-ZAR exchange rates.

---

## 📁 File Handling

PDF upload and download functionality was implemented for signed agreements with validation to ensure only valid PDF files are accepted.

---

## 🔍 Search and Filtering

LINQ-based filtering allows administrators to search contracts by:

* Date range
* Contract status

---

## ⚡ Async/Await Support

Asynchronous programming was implemented using `async` and `await` to improve:

* Database performance
* API responsiveness
* File upload efficiency

---

## 🎨 Responsive UI

Bootstrap was used to create a responsive and modern interface including:

* Dashboards
* Navigation menus
* Styled forms
* Responsive tables

---

# 🗄️ Database Design

The application uses **SQL Server** with the following primary tables:

* Clients
* Contracts
* ServiceRequests

### Contract Status Values

| Value | Status  |
| ----- | ------- |
| 0     | Draft   |
| 1     | Active  |
| 2     | Expired |
| 3     | OnHold  |

### Service Request Status Values

| Value | Status    |
| ----- | --------- |
| 0     | Pending   |
| 1     | Approved  |
| 2     | Completed |
| 3     | Cancelled |

---

# 🧪 Unit Testing

Automated testing was implemented using **xUnit**.

Tests include:

* Contract Workflow Service Tests
* Currency Service Tests
* File Validation Service Tests

---

# 🚀 Future Improvements

Potential future enhancements include:

* Microservices architecture
* Role-based authentication
* Cloud deployment using Microsoft Azure
* Real-time notifications
* Advanced reporting dashboards
* API integrations with logistics providers

---

# 📷 Screenshots

Add your screenshots here:

* Dashboard
* Client Management
* Contract Management
* Service Requests
* Unit Tests
* Database ERD

---

#  🔗 Links
* gituhub repo: https://github.com/AaliyahAllie/GLMS_AaliyahAllie_EAPD_ST10212542.git
* youtube demo link: 

---
# ⚙️ Installation

## 1. Clone the Repository

```bash
git clone https://github.com/your-username/glms.git
```

---

## 2. Open the Project

Open the solution in:

* Visual Studio 2022 or later

---

## 3. Configure Database

Update your `appsettings.json` connection string:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER;Database=GLMS_DB;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

---

## 4. Run Database Migrations

```bash
Update-Database
```

---

## 5. Run the Application

Press:

```bash
F5
```

or

```bash
Ctrl + F5
```

---

# 📘 Conclusion

The Global Logistics Management System successfully transformed manual logistics management processes into a centralized enterprise solution. By combining ASP.NET Core MVC, SQL Server, Entity Framework Core, workflow validation, external API integration, file handling, asynchronous programming, and automated testing, the system improves operational efficiency, compliance, and scalability for TechMove Logistics. 
