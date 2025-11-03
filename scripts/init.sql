-- Inicialização do banco de dados ShopSim
CREATE DATABASE IF NOT EXISTS ShopSim;
USE ShopSim;

-- Tabela Products
CREATE TABLE IF NOT EXISTS Products (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    Name VARCHAR(255) NOT NULL,
    Description TEXT,
    Price DECIMAL(10,2) NOT NULL,
    Stock INT NOT NULL DEFAULT 0,
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
);

-- Dados de exemplo
INSERT IGNORE INTO Products (Name, Description, Price, Stock) VALUES
('Produto Exemplo 1', 'Descrição do produto exemplo 1', 29.99, 100),
('Produto Exemplo 2', 'Descrição do produto exemplo 2', 49.99, 50),
('Produto Exemplo 3', 'Descrição do produto exemplo 3', 19.99, 200);
