# 🧑‍💼 Employee Management System

A complete employee registration and management system, built with:

✅ .NET 8 (ASP.NET Core)  
✅ Angular 18  
✅ PostgreSQL  
✅ Docker & Docker Compose  
✅ JWT Authentication  
✅ Role-based Authorization  

---

## 🚀 Getting Started

### 🔧 Prerequisites

- Docker

---

## ⚙️ Backend & Frontend Setup (via Docker)

You can spin up the API, Angular frontend, and PostgreSQL using Docker Compose:

**Important:**  
To run Docker Compose, you must be in the same directory where the `docker-compose.yml` file is located.

```bash
docker-compose up --build
```

Once the containers are running:

- 🧪 API (Swagger): [http://localhost:5000/swagger](http://localhost:5000/swagger)  
- 🌐 Frontend (Angular App): [http://localhost:4200](http://localhost:4200)

---

## 🔐 Default Admin Login

Use the following credentials to log in as the default admin:

- **Email:** `admin@example.com`  
- **Password:** `admin123`

---

## 👮 Roles & Permissions

Role-based access control is implemented:

- **Director**: Full access (create/view/edit/delete all employees)  
- **Leader**: Manage employees under their hierarchy  
- **Employee**: Limited access  

> ⚠️ You cannot create employees with a higher role than your own.

---

## 🧪 Features Summary

✅ JWT Authentication (Login, Logout, Role-based access)  
✅ Register and edit employees  
✅ Role selection restricted by current user's level  
✅ Manager selection via dropdown (linked employees)  
✅ Employee listing with edit/delete actions  
✅ API protected with Angular `AuthGuard`  
✅ Admin user seeded automatically in the database  

---

## 🧠 Architecture

- **Backend:** ASP.NET Core + EF Core + PostgreSQL  
- **Frontend:** Angular 18 + Angular Material  
- **Auth:** JWT  
- **Infra:** Docker + Docker Compose  

---

## 📝 Developer Notes

- EF Core migrations are applied automatically on backend startup.  
- The admin user is seeded via `OnModelCreating`.  
- Passwords are securely hashed using ASP.NET Identity’s password hasher.
