# Social API - CQRS com MediatR

API de rede social simplificada implementada com o padrão **CQRS (Command Query Responsibility Segregation)** utilizando .NET 8, MediatR e Entity Framework Core.

## Sobre o Padrão CQRS

O CQRS separa as operações de **leitura (Queries)** e **escrita (Commands)** em modelos distintos. Isso permite otimizar cada lado de forma independente, sendo especialmente benéfico quando o volume de leituras supera o de gravações.

> CQRS e padrões DDD não são estilos de arquitetura, mas sim padrões de arquitetura. Microsserviços, SOA e EDA (arquitetura orientada a eventos) são estilos arquitetônicos que descrevem um sistema de muitos componentes. O CQRS descreve algo dentro de um único sistema ou componente.

## Estrutura do Projeto

```
├── Social.API/                  # Camada de apresentação (ASP.NET Core Web API)
│   ├── Controllers/             # Controllers que despacham commands/queries via MediatR
│   └── Program.cs              # Configuração de DI, middleware e migrations
│
├── Social.Application/          # Camada de aplicação (orquestração)
│   ├── Commands/               # Definições de commands (escrita)
│   │   └── CommandHandlers/    # Handlers que processam os commands
│   ├── Queries/                # Definições de queries (leitura)
│   │   └── QueryHandlers/     # Handlers que processam as queries
│   └── Events/                 # (Reservado para eventos de domínio)
│
├── Social.Domain/              # Camada de domínio (entidades e regras de negócio)
│   └── Entities/              # Entidades com lógica de domínio rica
│
└── Social.Infrastructure/      # Camada de infraestrutura (persistência)
    ├── Persistence/           # DbContext e configuração do EF Core
    ├── Bus/                   # (Reservado para barramento de mensagens)
    └── Messaging/             # (Reservado para mensageria)
```

## Tecnologias

| Tecnologia | Versao | Finalidade |
|---|---|---|
| .NET 8 | net8.0 | Framework base |
| MediatR | 14.1.0 | Mediator para dispatching de commands/queries |
| Entity Framework Core | 8.0.0 | ORM e migrations |
| SQL Server | - | Banco de dados relacional |
| Swagger / Swashbuckle | 6.6.2 | Documentacao interativa da API |

## Endpoints da API

Base URL: `/api/v1/posts`

| Metodo | Rota | Descricao |
|---|---|---|
| POST | `/api/v1/posts` | Criar um novo post |
| GET | `/api/v1/posts/user/{userId}` | Listar posts de um usuario |
| GET | `/api/v1/posts/{postId}` | Buscar post por ID |

### Exemplos de Uso

**Criar Post:**
```http
POST /api/v1/posts
Content-Type: application/json

{
  "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "message": "Meu primeiro post!"
}
```

**Buscar Posts de um Usuario:**
```http
GET /api/v1/posts/user/3fa85f64-5717-4562-b3fc-2c963f66afa6
```

**Buscar Post por ID:**
```http
GET /api/v1/posts/550e8400-e29b-41d4-a716-446655440000
```

## Regras de Dominio (Post)

- O post deve ter um usuario criador (UserId nao pode ser vazio)
- A mensagem e obrigatoria
- A mensagem nao pode exceder 140 caracteres
- Cada post e criado com 0 likes e flag `Deleted = false`

## Fluxo CQRS na Aplicacao

```
Controller -> MediatR.Send(Command/Query)
                        |
         +--------------+--------------+
         |                             |
    [COMMAND]                     [QUERY]
         |                             |
  CommandHandler               QueryHandler
         |                             |
  DbContext.Add()           DbContext.Find()
  DbContext.SaveChanges()   DbContext.Include()
```

## Como Executar

### Pre-requisitos

- .NET 8 SDK
- SQL Server (local ou via Docker)

### Configurar banco de dados

A connection string esta em `Social.API/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=SocialDb;User Id=sa;Password=Your_password123;TrustServerCertificate=True;"
  }
}
```

### Executar a aplicacao

```bash
cd Social.API
dotnet run
```

A aplicacao executa as migrations automaticamente ao iniciar. O Swagger estara disponivel em `https://localhost:{porta}/swagger` no ambiente de desenvolvimento.

### Subir SQL Server via Docker (opcional)

```bash
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=Your_password123" -p 1433:1433 -d mcr.microsoft.com/mssql/server:2022-latest
```

## Referencia de Projetos

```
Social.API
  └── Social.Application
  │     ├── Social.Domain
  │     └── Social.Infrastructure
  │           └── Social.Domain
  └── Social.Infrastructure
        └── Social.Domain
```
