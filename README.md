# Pokemon Review App 🧩
A full-featured ASP.NET Core Web API built with Entity Framework Core, SQL Server, and Swagger UI.  
This project allows users to manage Pokémon data, categories, reviews, and ownership details — extended with additional relations such as Food and Properties.

### 🛠️ Tech Stack
- ASP.NET Core 6.0 Web API
- Entity Framework Core
- SQL Server Express
- Swagger UI (OpenAPI 3.0)
- AutoMapper
- Repository & DTO Pattern

### ⚙️ Key Features
- Full CRUD operations via RESTful endpoints
- Many-to-many relationships between Pokémon, Owners, Categories, Foods, and Properties
- Clean architecture with layered design (Controllers, Repositories, DTOs)
- Database migrations and seeding
- API documentation and testing through Swagger UI

### 📦 Database Schema
- Pokémon ↔ Category  
- Pokémon ↔ Owner  
- Pokémon ↔ Food  
- Pokémon ↔ Property  

### 🚀 Future Plans
- Authentication (JWT)
- Pagination and filtering
- Error handling middleware
