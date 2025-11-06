# 🛒 ShopSim - Enterprise E-commerce API

A comprehensive, enterprise-grade e-commerce API built with ASP.NET Core 8.0, featuring advanced authentication, order management, and modern architecture patterns.

[![.NET](https://img.shields.io/badge/.NET-8.0-blue.svg)](https://dotnet.microsoft.com/download/dotnet/8.0)
[![MySQL](https://img.shields.io/badge/MySQL-8.0-orange.svg)](https://www.mysql.com/)
[![Docker](https://img.shields.io/badge/Docker-Supported-blue.svg)](https://www.docker.com/)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)

## 🚀 Features

### 🔐 Authentication & Authorization
- **JWT Token Authentication**: Secure user authentication with JSON Web Tokens
- **Role-based Access Control**: Admin and Customer roles with different permissions
- **User Management**: Registration, login, and profile management
- **Password Security**: BCrypt password hashing

### 📦 Product Management
- **Advanced Product CRUD**: Complete product lifecycle management
- **Category Organization**: Products organized by categories
- **Stock Management**: Real-time inventory tracking
- **Advanced Filtering**: Search by name, category, price range, and stock status
- **Pagination & Sorting**: Efficient data retrieval with customizable pagination

### 🛍️ Order Processing
- **Shopping Cart Functionality**: Add/remove items from cart
- **Order Management**: Complete order lifecycle from creation to delivery
- **Order Status Tracking**: Pending, Processing, Shipped, Delivered, Cancelled
- **Inventory Integration**: Automatic stock updates on order placement
- **Order History**: Complete order tracking for users

### 🏗️ Architecture & Quality
- **Clean Architecture**: Separation of concerns with clear layer boundaries
- **Repository Pattern**: Data access abstraction
- **AutoMapper Integration**: Efficient object-to-object mapping
- **Global Exception Handling**: Centralized error management
- **Response Standardization**: Consistent API response format
- **Comprehensive Logging**: Structured logging for monitoring

## 🛠️ Technology Stack

- **Framework**: ASP.NET Core 8.0
- **Database**: MySQL 8.0
- **ORM**: Entity Framework Core
- **Authentication**: JWT Bearer Tokens
- **Password Hashing**: BCrypt.Net
- **Mapping**: AutoMapper
- **Containerization**: Docker & Docker Compose
- **Documentation**: Swagger/OpenAPI

## 🏃‍♂️ Quick Start

### Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker](https://www.docker.com/get-started)
- [Docker Compose](https://docs.docker.com/compose/)

### 🐳 Running with Docker (Recommended)

1. **Clone the repository:**
```bash
git clone https://github.com/RenanCruz7/ShopSim.git
cd ShopSim
```

2. **Start the application:**
```bash
docker-compose up -d
```

3. **Access the application:**
- **API**: http://localhost:5000
- **Swagger UI**: http://localhost:5000 (Interactive API documentation)

## 📡 API Endpoints

### 🔑 Authentication
```
POST /api/auth/register    - Register new user
POST /api/auth/login       - User login
GET  /api/auth/me          - Get current user info
```

### 📦 Products
```
GET    /api/products              - Get products (with filtering & pagination)
GET    /api/products/{id}         - Get product by ID
POST   /api/products              - Create new product [Auth Required]
PUT    /api/products/{id}         - Update product [Auth Required]
DELETE /api/products/{id}         - Delete product [Auth Required]
GET    /api/products/category/{id} - Get products by category
```

### 🏷️ Categories
```
GET    /api/categories           - Get all categories (with pagination)
GET    /api/categories/{id}      - Get category by ID
POST   /api/categories           - Create new category [Auth Required]
PUT    /api/categories/{id}      - Update category [Auth Required]
DELETE /api/categories/{id}      - Delete category [Auth Required]
```

### 🛍️ Orders
```
GET    /api/orders              - Get all orders [Admin Only]
GET    /api/orders/user/{id}    - Get user orders [Auth Required]
GET    /api/orders/{id}         - Get order by ID [Auth Required]
POST   /api/orders/user/{id}    - Create new order [Auth Required]
PUT    /api/orders/{id}/status  - Update order status [Admin Only]
DELETE /api/orders/{id}/user/{userId} - Cancel order [Auth Required]
```

## 🔐 Authentication Example

1. **Register:**
```json
POST /api/auth/register
{
  "firstName": "John",
  "lastName": "Doe", 
  "email": "john@example.com",
  "password": "SecurePass123!"
}
```

2. **Login:**
```json  
POST /api/auth/login
{
  "email": "john@example.com",
  "password": "SecurePass123!"
}
```

3. **Use token:**
```
Authorization: Bearer your-jwt-token-here
```

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch
3. Commit your changes
4. Push to the branch
5. Create a Pull Request

## 👨‍💻 Author

**Renan Cruz**
- GitHub: [@RenanCruz7](https://github.com/RenanCruz7)

---

⭐ **Star this repository if it helped you!**

## Como executar com Docker

### Pré-requisitos
- Docker Desktop instalado
- Docker Compose instalado

### Executando o projeto

1. **Clone o repositório e navegue até a pasta do projeto:**
   ```bash
   cd C:\Users\Renan\RiderProjects\ShopSim
   ```

2. **Execute com Docker Compose:**
   ```bash
   docker-compose up --build
   ```

3. **Acesse a aplicação:**
   - API: http://localhost:8080
   - Swagger: http://localhost:8080/swagger
   - phpMyAdmin: http://localhost:8081

### Comandos úteis

- **Executar em background:**
  ```bash
  docker-compose up -d --build
  ```

- **Parar os serviços:**
  ```bash
  docker-compose down
  ```

- **Ver logs:**
  ```bash
  docker-compose logs -f shopsim-api
  ```

- **Reconstruir apenas a API:**
  ```bash
  docker-compose build shopsim-api
  ```

## Estrutura do Projeto

```
ShopSim/
├── Controllers/        # Controladores da API
├── Data/              # Context do Entity Framework
├── DTOs/              # Data Transfer Objects
├── Models/            # Modelos de dados
├── Profiles/          # Perfis do AutoMapper
├── Services/          # Serviços de negócio
├── scripts/           # Scripts SQL
├── Dockerfile         # Configuração Docker
└── docker-compose.yml # Orquestração de serviços
```

## Configuração do Banco de Dados

O banco MySQL é automaticamente configurado com:
- **Database:** ShopSim
- **Usuário:** appuser
- **Senha:** AppPassword123!
- **Dados de exemplo** são inseridos automaticamente

## Endpoints da API

- `GET /api/products` - Lista todos os produtos
- `GET /api/products/{id}` - Obtém um produto específico
- `POST /api/products` - Cria um novo produto
- `PUT /api/products/{id}` - Atualiza um produto
- `DELETE /api/products/{id}` - Remove um produto
