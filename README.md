🧑‍💼 Employee Management System
A complete employee registration and management system, built with:

✅ .NET 8 (ASP.NET Core)

✅ Angular 18

✅ PostgreSQL

✅ Docker & Docker Compose

✅ JWT Authentication

✅ Role-based Authorization

🚀 Getting Started
🔧 Prerequisites

Docker
Node.js & npm
Angular CLI

⚙️ Backend Setup (.NET API)
🐳 Run via Docker
You can spin up the API and PostgreSQL using Docker Compose:

**Important:**  
To run Docker Compose, you must be in the same directory where the `docker-compose.yml` file is located:


```bash
docker-compose up --build
```
Once the containers are up, the API will be available at:

```bash
http://localhost:5000/swagger
```
✅ Swagger UI will help you test and explore the available endpoints.

🖥️ Frontend Setup (Angular)
Navigate to the frontend project folder:

```bash
cd employee-management-client
```
Install dependencies:

```bash
npm install
```
Run the application:

```bash
ng serve
```
Access in your browser:

```
http://localhost:4200
```
🔐 Default Admin Login
Use the following credentials to log in as the default admin:

Email: admin@example.com

Password: admin123

👮 Roles & Permissions
Role-based access control is implemented:

Director can create and view all employees.

Leader can manage employees under their hierarchy.

Employee has limited permissions.

You cannot create employees with a higher role than your own.

🧪 Features Summary
✅ JWT Authentication (Login, Logout, Role-based access)

✅ Register and edit employees

✅ Role selection restricted by current user's level

✅ Manager selection via dropdown (linked employees)

✅ Employee listing with edit/delete actions

✅ API protected with AuthGuard on Angular side

✅ Admin seeded automatically in database

🧠 Architecture
Backend: ASP.NET Core + EF Core + PostgreSQL

Frontend: Angular 18 + Angular Material

Auth: JWT

Infra: Docker + Docker Compose

📝 Developer Notes
The backend applies EF Core Migrations at startup.

The admin employee is seeded in the database via the OnModelCreating method.

Passwords are hashed using ASP.NET’s built-in hasher.
