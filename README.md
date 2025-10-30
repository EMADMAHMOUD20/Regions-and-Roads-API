# 🛣️ Roads & Regions API

This project is a **.NET 9 Web API** built with **Clean Architecture**, designed to manage roads and regions.  
It allows users to view all roads that reach a specific region and supports secure authentication using **JWT and Refresh Tokens**.

---

## 🚀 Features

- ✅ **Clean Architecture**
  - Controller → Service → Repository → Database
  - Clear separation of concerns

- 🔐 **Authentication**
  - Login & Register using **ASP.NET Core Identity**
  - **JWT tokens** for secure access
  - **Refresh tokens** to renew access tokens safely

- 🌍 **Roads and Regions Management**
  - Add, update, and delete roads and regions
  - Retrieve all roads that connect to a specific region

- 🧩 **Validation**
  - Input validation

- 📨 **Email Verification**
  - Send OTP to email and verify before access

- 🧾 **Logging**
  - Integrated logging with **Serilog**
  - Logs saved to file and console

---

## 🧱 Architecture Overview

- **Controller Layer:** handles HTTP requests and responses  
- **Service Layer:** contains business logic  
- **Repository Layer:** interacts with the database using Entity Framework Core  
- **Infrastructure Layer:** includes configurations for EF Core, Identity, JWT, logging, etc.

---

## ⚙️ Technologies Used

- **ASP.NET Core 9**
- **Entity Framework Core**
- **SQL Server**
- **ASP.NET Identity**
- **JWT Authentication + Refresh Tokens**
- **Serilog** for logging
- **Swagger** for API documentation

---

## 🔧 How to Run the Project

1. **Clone the repository**
   ```bash
   git clone https://github.com/EMADMAHMOUD20/Regions-and-Roads-API.git

