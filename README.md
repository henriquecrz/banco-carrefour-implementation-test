# banco-carrefour-implementation-test

## O que foi feito

Desenvolvimento de uma API escrita em .NET que faz o controle de lançamentos (crédito e débito) e retorna o saldo diário consolidado a partir de uma determinada data.

## Rotas

- GET /Balance: retorna todos os lançamentos criados em uma determinada data agrupados pelo tipo da moeda (ex: BRL, USD, etc)
  - Parâmetros: date, formato ano-mês-dia (ex: 2023-05-12)
- GET /CashFlow: retorna todos os lançamentos criados
- POST /CashFlow: cria um lançamento
  - Parâmetros:
    - id: int
    - type: int (Enum)
      - Crédito: 0
      - Débito: 1
    - currency: int (Enum)
      - BRL: 0
      - USD: 1
      - EUR: 2
    - value: double (decimal)
    - description: string
    - date: string (DateTime)

## Tecnologias

- C#
- .NET 6.0
- Entity Framework Core
- LINQ
- MySQL
- Swagger
- Singleton
- Clean Code
- SOLID

## Desenho da solução

![Desenho da Solução](https://github.com/henriquecrz/banco-carrefour-implementation-test/blob/main/Desenho%20da%20Solu%C3%A7%C3%A3o.png)

## Instruções

- Instalar Docker na máquina: https://www.docker.com/products/docker-desktop
- Baixar a imagem do MySQL do Docker Hub: docker pull mysql
- Executar a instância de banco de dados MySQL: docker run --name mysql-container -e MYSQL_ROOT_PASSWORD=a -e MYSQL_DATABASE=cashflow -p 3306:3306 -d mysql
- No path raiz executar o comando: Update-Database
- Navegar até o path cash-flow/api/ e executar o comando: dotnet build
- No mesmo path executar: dotnet run
- Acessar Swagger: http://localhost:5129/swagger/index.html

## O que falta a ser desenvolvido

- Comunicação (network) entre o container da aplicação e do banco de dados MySQL
- Mensageria
- Health check
- Log
- Teste de integração
