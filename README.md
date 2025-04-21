# ASP.NET Web API

## Overview
**NetSolutions API** is a robust and secure backend service designed to power the Client Portal, a Vue.js application built with the Composition API. This API provides authentication, data management, document handling, and communication functionalities essential for seamless user experiences.

## Features
- **User Authentication & Authorization**
  - JWT-based authentication
  - Role-based access control (RBAC)
- **Dashboard API**
  - Real-time status updates
  - Analytics and insights
- **User Management**
  - CRUD operations for user profiles
  - Password reset and account security
- **Project Tracking**
  - Retrieve and update project statuses
  - Assign tasks and manage project progress
- **Document Management**
  - Upload, retrieve, and manage documents securely
  - Version control and access permissions
- **Messaging & Notifications**
  - Secure communication channels between users
  - Email and push notifications

## 🧹 Tech Stack

### 🔧 Core Frameworks & ORM
- **Backend Framework:** ASP.NET Core Web API
- **ORM:** Entity Framework Core (SQL Server / SQLite)
- **Authentication:** ASP.NET Identity with JWT

> 🛠️ Key NuGet Packages:
> - Microsoft.AspNetCore.Authentication.JwtBearer
> - Microsoft.AspNetCore.Identity.EntityFrameworkCore
> - Microsoft.EntityFrameworkCore.SqlServer
> - Microsoft.EntityFrameworkCore.Sqlite

### 🧠 Utilities
- **Object Mapping:** AutoMapper
- **Test Data Generation:** Bogus

### 🧵 Caching & State Management
- **Redis:** via Microsoft.Extensions.Caching.StackExchangeRedis

### ☁️ Cloud Storage
- **Media Hosting:** Cloudinary
- **File Storage:** Google Drive API

### 📧 Emailing & Templates
- **SMTP & MIME Handling:** MailKit, MimeKit
- **Templating Engine:** RazorLight

### 📜 API Documentation
- **Swagger UI:** Swashbuckle.AspNetCore

### 🌱 Configuration Management
- **Environment Variable Support:** dotenv.net

### 📈 Logging & Monitoring
- **Logging:** Serilog
- **Telemetry:** Application Insights

### 🚢 Deployment
- **Containerization:** Docker
- **Orchestration (Optional):** Kubernetes

## 📡 API Endpoints

| Endpoint                   | Method | Description                           | Authentication |
|----------------------------|--------|---------------------------------------|----------------|
| `/api/auth/login`          | POST   | Authenticate user and return JWT     | No             |
| `/api/auth/register`       | POST   | Register a new user                  | No             |
| `/api/auth/logout`         | POST   | Logout and invalidate session        | Yes            |
| `/api/users/{id}`          | GET    | Retrieve user profile                | Yes            |
| `/api/users/update`        | PUT    | Update user profile                  | Yes            |
| `/api/projects`            | GET    | Get all projects                     | Yes            |
| `/api/projects/{id}`       | GET    | Get a single project                 | Yes            |
| `/api/projects/create`     | POST   | Create a new project                 | Yes (Admin)    |
| `/api/documents/upload`    | POST   | Upload a document                    | Yes            |
| `/api/notifications`       | GET    | Fetch notifications                  | Yes            |

## 🛠 Development Setup

### Prerequisites
- .NET SDK 8.0+
- SQL Server / PostgreSQL
- Redis (for caching, optional)
- Node.js (for frontend integration testing)

---

## 📄 License
This project is licensed under the **MIT License**.


