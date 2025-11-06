# 🧪 Guia de Testes - ShopSim API

## Como Testar Todas as Funcionalidades Implementadas

### 1. 🚀 Iniciar a Aplicação

**Opção A - Com Docker (Recomendado):**
```bash
cd ShopSim
docker-compose up -d
```

**Opção B - Localmente:**
```bash
cd ShopSim/ShopSim
dotnet ef database update
dotnet run
```

**Acessar:**
- API: http://localhost:5000
- Swagger UI: http://localhost:5000

### 2. 🔐 Testar Autenticação

#### 2.1 Registrar Usuário
```http
POST /api/auth/register
Content-Type: application/json

{
  "firstName": "João",
  "lastName": "Silva", 
  "email": "joao@example.com",
  "password": "MinhaSenh@123"
}
```

#### 2.2 Fazer Login
```http
POST /api/auth/login
Content-Type: application/json

{
  "email": "joao@example.com",
  "password": "MinhaSenh@123"
}
```

**Copie o token da resposta para usar nos próximos requests!**

### 3. 🏷️ Testar Categorias

#### 3.1 Criar Categoria
```http
POST /api/categories
Content-Type: application/json
Authorization: Bearer SEU_TOKEN_AQUI

{
  "name": "Eletrônicos",
  "description": "Dispositivos eletrônicos e gadgets"
}
```

#### 3.2 Listar Categorias (com paginação)
```http
GET /api/categories?page=1&pageSize=5&searchTerm=eletr&sortBy=name&sortDirection=asc
```

#### 3.3 Buscar Categoria por ID
```http
GET /api/categories/1
```

### 4. 📦 Testar Produtos

#### 4.1 Criar Produto
```http
POST /api/products
Content-Type: application/json
Authorization: Bearer SEU_TOKEN_AQUI

{
  "name": "Smartphone Samsung Galaxy",
  "description": "Smartphone com tela AMOLED de 6.1 polegadas",
  "price": 899.99,
  "stockQuantity": 50,
  "categoryId": 1,
  "imageUrl": "https://example.com/samsung.jpg",
  "sku": "SAMSUNG-001"
}
```

#### 4.2 Listar Produtos (com filtros avançados)
```http
GET /api/products?page=1&pageSize=10&searchTerm=samsung&categoryId=1&minPrice=500&maxPrice=1000&inStock=true&sortBy=price&sortDirection=desc
```

#### 4.3 Buscar Produtos por Categoria
```http
GET /api/products/category/1
```

#### 4.4 Atualizar Produto
```http
PUT /api/products/1
Content-Type: application/json
Authorization: Bearer SEU_TOKEN_AQUI

{
  "name": "Smartphone Samsung Galaxy S24",
  "description": "Smartphone premium com tela AMOLED de 6.1 polegadas",
  "price": 1199.99,
  "stockQuantity": 30,
  "categoryId": 1,
  "imageUrl": "https://example.com/samsung-s24.jpg",
  "sku": "SAMSUNG-S24",
  "isActive": true
}
```

### 5. 🛍️ Testar Pedidos

#### 5.1 Criar Pedido
```http
POST /api/orders/user/1
Content-Type: application/json
Authorization: Bearer SEU_TOKEN_AQUI

{
  "items": [
    {
      "productId": 1,
      "quantity": 2
    }
  ],
  "shippingAddress": "Rua das Flores, 123, São Paulo, SP, 01234-567"
}
```

#### 5.2 Listar Pedidos do Usuário
```http
GET /api/orders/user/1?page=1&pageSize=10
Authorization: Bearer SEU_TOKEN_AQUI
```

#### 5.3 Buscar Pedido por ID
```http
GET /api/orders/1
Authorization: Bearer SEU_TOKEN_AQUI
```

#### 5.4 Atualizar Status do Pedido (Admin)
```http
PUT /api/orders/1/status
Content-Type: application/json
Authorization: Bearer SEU_TOKEN_AQUI

{
  "status": "Processing"
}
```

### 6. 🧪 Testar Casos de Erro

#### 6.1 Tentar Acessar Endpoint Protegido sem Token
```http
GET /api/products
# Deve retornar 401 Unauthorized
```

#### 6.2 Tentar Criar Produto com Dados Inválidos
```http
POST /api/products
Content-Type: application/json
Authorization: Bearer SEU_TOKEN_AQUI

{
  "name": "",
  "price": -10,
  "categoryId": 999
}
# Deve retornar 400 Bad Request com detalhes dos erros
```

#### 6.3 Buscar Recurso Inexistente
```http
GET /api/products/999
# Deve retornar 404 Not Found
```

### 7. 📊 Testar Paginação e Filtros

#### 7.1 Paginação
```http
GET /api/products?page=2&pageSize=5
```

#### 7.2 Busca por Texto
```http
GET /api/products?searchTerm=samsung
```

#### 7.3 Filtro por Preço
```http
GET /api/products?minPrice=100&maxPrice=500
```

#### 7.4 Produtos em Estoque
```http
GET /api/products?inStock=true
```

#### 7.5 Ordenação
```http
GET /api/products?sortBy=price&sortDirection=desc
```

## 🎯 Casos de Teste Específicos

### Teste 1: Fluxo Completo de Compra
1. Registrar usuário
2. Fazer login
3. Criar categoria
4. Criar produto na categoria
5. Criar pedido com o produto
6. Verificar redução do estoque
7. Atualizar status do pedido

### Teste 2: Validação de Permissões
1. Tentar acessar endpoints sem autenticação
2. Tentar operações administrativas com usuário comum
3. Verificar acesso apenas aos próprios pedidos

### Teste 3: Integridade de Dados
1. Tentar criar produto em categoria inexistente
2. Tentar fazer pedido com produto sem estoque
3. Tentar deletar categoria com produtos associados

## 📱 Testando no Swagger UI

1. Acesse: http://localhost:5000
2. Clique em "Authorize" no topo da página
3. Faça login via `/api/auth/login`
4. Copie o token da resposta
5. Cole no campo "Value" como: `Bearer SEU_TOKEN`
6. Clique "Authorize"
7. Agora você pode testar todos os endpoints protegidos!

## 🚀 Funcionalidades Demonstradas

✅ **Autenticação JWT completa**
✅ **CRUD completo para todas as entidades**  
✅ **Paginação e filtros avançados**
✅ **Relacionamentos entre entidades**
✅ **Controle de estoque em tempo real**
✅ **Tratamento de erros globalizado**
✅ **Validação de dados robusta**
✅ **Documentação interativa**
✅ **Arquitetura escalável**
✅ **Padrões de API profissionais**

## 🎉 Resultado

Seu projeto ShopSim agora demonstra competência em:
- Backend development com .NET Core
- API design e REST principles
- Database design e Entity Framework
- Authentication e Authorization
- Error handling e validation
- Documentation e testing
- Clean architecture principles

**Perfect for showcasing in your portfolio! 🌟**
