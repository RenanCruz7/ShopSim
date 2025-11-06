﻿# 🛒 ShopSim - E-commerce API

Uma API REST moderna para e-commerce desenvolvida com **ASP.NET Core 8**, featuring autenticação JWT, gerenciamento de produtos, categorias e pedidos.

[![.NET](https://img.shields.io/badge/.NET-8.0-blue.svg)](https://dotnet.microsoft.com/download/dotnet/8.0)
[![MySQL](https://img.shields.io/badge/MySQL-8.0-orange.svg)](https://www.mysql.com/)
[![Docker](https://img.shields.io/badge/Docker-Supported-blue.svg)](https://www.docker.com/)

## ✨ Funcionalidades

### 🔐 Autenticação e Autorização
- **JWT Authentication** com tokens seguros
- **Controle de acesso** baseado em roles (Admin/Customer)
- **Registro e login** de usuários
- **Criptografia de senhas** com BCrypt

### 📦 Gerenciamento de Produtos
- **CRUD completo** de produtos
- **Organização por categorias**
- **Controle de estoque** em tempo real
- **Filtros avançados** (nome, categoria, preço, estoque)
- **Paginação e ordenação**

### 🛍️ Sistema de Pedidos
- **Carrinho de compras**
- **Processamento de pedidos** completo
- **Status de pedidos** (Pendente, Processando, Enviado, Entregue, Cancelado)
- **Histórico de pedidos** por usuário

### 🏗️ Arquitetura
- **Clean Architecture** com separação de responsabilidades
- **Global Exception Handling**
- **Padronização de respostas** da API
- **AutoMapper** para mapeamento de objetos
- **Documentação Swagger** completa

## 🛠️ Tecnologias

- **ASP.NET Core 8.0**
- **Entity Framework Core**
- **MySQL 8.0**
- **JWT Bearer Authentication**
- **BCrypt.Net**
- **AutoMapper**
- **Docker & Docker Compose**
- **Swagger/OpenAPI**

## 🚀 Como Executar

### Pré-requisitos
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker](https://www.docker.com/get-started)

### 🐳 Com Docker (Recomendado)

1. **Clone o repositório:**
```bash
git clone https://github.com/RenanCruz7/ShopSim.git
cd ShopSim
```

2. **Execute a aplicação:**
```bash
docker-compose up -d
```

3. **Acesse:**
- **API**: http://localhost:5000
- **Swagger**: http://localhost:5000

### 💻 Executar Localmente

1. **Configure o banco:**
   - Certifique-se que o MySQL está rodando
   - Ajuste a connection string se necessário

2. **Execute as migrations:**
```bash
cd ShopSim
dotnet ef database update
```

3. **Inicie a aplicação:**
```bash
dotnet run
```

## 📚 API Endpoints

### 🔑 Autenticação
- `POST /api/auth/register` - Registrar usuário
- `POST /api/auth/login` - Login
- `GET /api/auth/me` - Informações do usuário

### 📦 Produtos
- `GET /api/products` - Listar produtos (com filtros)
- `GET /api/products/{id}` - Buscar produto por ID
- `POST /api/products` - Criar produto [Auth]
- `PUT /api/products/{id}` - Atualizar produto [Auth]
- `DELETE /api/products/{id}` - Deletar produto [Auth]

### 🏷️ Categorias
- `GET /api/categories` - Listar categorias
- `GET /api/categories/{id}` - Buscar categoria por ID
- `POST /api/categories` - Criar categoria [Auth]
- `PUT /api/categories/{id}` - Atualizar categoria [Auth]
- `DELETE /api/categories/{id}` - Deletar categoria [Auth]

### 🛍️ Pedidos
- `GET /api/orders` - Listar pedidos [Admin]
- `GET /api/orders/user/{id}` - Pedidos do usuário [Auth]
- `GET /api/orders/{id}` - Buscar pedido por ID [Auth]
- `POST /api/orders/user/{id}` - Criar pedido [Auth]
- `PUT /api/orders/{id}/status` - Atualizar status [Admin]

## 💡 Como Testar

1. **Acesse o Swagger**: http://localhost:5000
2. **Registre um usuário** em `/api/auth/register`
3. **Faça login** em `/api/auth/login`
4. **Copie o token** e clique em "Authorize"
5. **Cole como**: `Bearer seu-token-aqui`
6. **Teste os endpoints** protegidos!

### Exemplo de Registro
```json
POST /api/auth/register
{
  "firstName": "João",
  "lastName": "Silva",
  "email": "joao@exemplo.com",
  "password": "MinhaSenh@123"
}
```

### Exemplo de Produto
```json
POST /api/products
{
  "name": "Smartphone",
  "description": "Smartphone com 128GB",
  "price": 899.99,
  "stockQuantity": 50,
  "categoryId": 1,
  "sku": "PHONE-001"
}
```

## 📁 Estrutura do Projeto

```
ShopSim/
├── Controllers/         # Controllers da API
├── Services/           # Lógica de negócio
├── Models/             # Entidades do banco
├── DTOs/               # Data Transfer Objects
├── Data/               # Contexto do EF Core
├── Profiles/           # Profiles do AutoMapper
├── Middleware/         # Middlewares customizados
└── Migrations/         # Migrations do banco
```

## 🎯 Características Técnicas

- ✅ **Arquitetura limpa** e escalável
- ✅ **Tratamento global** de exceções
- ✅ **Validação** de dados robusta
- ✅ **Paginação** e filtros avançados
- ✅ **Documentação** interativa
- ✅ **Containerização** com Docker
- ✅ **Segurança** com JWT e BCrypt

## 🤝 Contribuindo

1. Faça um Fork do projeto
2. Crie uma branch para sua feature
3. Commit suas mudanças
4. Push para a branch
5. Abra um Pull Request

## 👨‍💻 Autor

**Renan Cruz**
- GitHub: [@RenanCruz7](https://github.com/RenanCruz7)

---

⭐ **Deixe uma estrela se este projeto te ajudou!**

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
