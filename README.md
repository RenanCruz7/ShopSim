# ShopSim - API de Simulação de Loja

Este é um projeto de API REST desenvolvido em .NET 8 para simulação de uma loja.

## Tecnologias Utilizadas

- .NET 8
- Entity Framework Core
- MySQL 8.0
- AutoMapper
- Docker & Docker Compose

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
