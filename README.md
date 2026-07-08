# Arquiteturas — Projetos de Estudo

Repositório com projetos acadêmicos para praticar **arquiteturas de software**, **padrões arquiteturais** e **design patterns** usando **.NET 8**.

Cada projeto é autocontido, documentado e construído em torno de um caso de uso realista para tornar o estudo concreto, não apenas teórico.

---

## Projetos

### 1. Clean Architecture — Sistema de Pedidos

**Pasta:** `Clean Architecture/`

Sistema de gerenciamento de pedidos (produtos, cupons de desconto e pedidos) com UI em **Blazor Server**.

O foco é demonstrar como a Clean Architecture isola as regras de negócio da infraestrutura, tornando o domínio 100% independente de tecnologia e testável sem banco ou UI.

**Camadas:** `Order.Domain` → `Order.Application` → `Order.Infrastructure` + `Order.CrossCutting` + `Order.Blazor`

**Padrões utilizados:**
- Clean Architecture (Uncle Bob)
- DDD — entidades ricas, Aggregate Root, Factory Methods
- Result Pattern — erros sem exceções
- Repository Pattern
- Dependency Injection / Composition Root

**Infraestrutura:** EF Core InMemory (sem dependências externas)

---

### 2. CQRS — Rede Social

**Pasta:** `CQRS/`

API de uma rede social (`Social.API`) que separa o modelo de escrita do modelo de leitura com **CQRS** puro via **MediatR**.

O foco é mostrar como Commands e Queries evoluem de forma independente, e como o CQRS se encaixa dentro de um único serviço sem exigir Event Sourcing ou múltiplos bancos.

**Camadas:** `Social.Domain` → `Social.Application` (Commands/Queries/Events) → `Social.Infrastructure` (Bus/Persistence/Messaging) + `Social.API`

**Padrões utilizados:**
- CQRS
- Mediator (MediatR)
- Command/Query Handlers
- Domain Events

---

### 3. CQRS + Event Sourcing — Carteira Digital (EWallet)

**Pasta:** `CQRS + EventSourcing/`

Carteira digital com operações de criar conta, depositar, transferir, reembolsar e bloquear. É o projeto mais completo do repositório em termos de infraestrutura.

O estado de cada conta **nunca é salvo diretamente** — ele é sempre reconstruído (rehidratado) a partir de uma sequência de eventos imutáveis no Event Store. O read model é construído de forma assíncrona por um Worker que consome eventos via RabbitMQ e projeta no MongoDB.

**Projetos:** `EWallet.Domain` · `EWallet.Application` · `EWallet.Infrastructure` · `EWallet.Blazor` · `EWallet.Worker` · `EWallet.Worker.EventStore`

**Padrões utilizados:**
- CQRS
- Event Sourcing
- Mediator (MediatR)
- Projection / Read Model
- Optimistic Locking
- Strategy Pattern (projeções)

**Infraestrutura:** SQL Server (Event Store) + MongoDB (Read Model) + RabbitMQ (mensageria)

Para subir as dependências com Docker:
```powershell
.\start-dev.ps1
```

---

### 4. Hexagonal — Sistema de Notificações

**Pasta:** `Hexagonal/`

Sistema de envio de notificações por múltiplos canais (Email, SMS e WhatsApp) com UI em **Blazor Server**. Os gateways de envio são simulados via log, o que demonstra a troca de adaptadores sem tocar no núcleo.

O foco é mostrar a separação entre **portas** (interfaces do núcleo) e **adaptadores** (implementações concretas), onde adicionar um novo canal significa criar uma fábrica + um gateway e registrar no DI — sem alterar nenhuma regra de negócio.

**Camadas:** `Notification.Domain` → `Notification.Application` → `Notification.Infrastructure` + `Notification.Api` (Blazor)

**Padrões utilizados:**
- Ports & Adapters (Arquitetura Hexagonal — Alistair Cockburn)
- Template Method — validação por canal com hook opcional
- Abstract Factory — família de produtos por canal
- Facade / Resolver — seleção de implementação por nome de canal
- Strategy (implícito) via `INotificationGateway`
- Dependency Injection / Composition Root

**Infraestrutura:** Nenhuma — sem banco, sem fila, apenas logs

---

## Stack Comum

| Tecnologia | Uso |
|---|---|
| .NET 8 | Framework base de todos os projetos |
| Blazor Server | UI nos projetos Clean Architecture, Hexagonal e EWallet |
| MediatR | Dispatcher de Commands/Queries nos projetos CQRS |
| EF Core 8 | ORM (InMemory ou SQL Server dependendo do projeto) |
| AutoMapper | Mapeamento entidade → DTO no Clean Architecture |
| SQL Server | Event Store no EWallet |
| MongoDB | Read Model no EWallet |
| RabbitMQ | Mensageria no EWallet |
| Docker Compose | Infraestrutura local para o EWallet |

---

## Como navegar

Cada pasta tem seu próprio `README.md` com:
- Explicação do padrão arquitetural
- Diagrama de camadas e fluxo de dados
- Padrões de projeto utilizados e onde
- Trade-offs e quando usar (ou não)
- Instruções para rodar

O `docker-compose.yml` e o `start-dev.ps1` na raiz são usados exclusivamente pelo projeto **CQRS + Event Sourcing**.
