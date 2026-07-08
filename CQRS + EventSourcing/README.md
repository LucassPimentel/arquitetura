# EWallet — CQRS + Event Sourcing

Projeto de estudo e referência de uma carteira digital (EWallet) construída com **.NET 8**, aplicando as arquiteturas **CQRS** (Command Query Responsibility Segregation) e **Event Sourcing** de forma completa e deliberada.

---

## Sumário

- [Padrões Arquiteturais](#padrões-arquiteturais)
  - [CQRS](#cqrs-command-query-responsibility-segregation)
  - [Event Sourcing](#event-sourcing)
  - [CQRS + Event Sourcing juntos](#cqrs--event-sourcing-juntos)
- [Visão Geral](#visão-geral)
- [Arquitetura](#arquitetura)
  - [Diagrama de Projetos](#diagrama-de-projetos)
  - [Fluxo Write Side (Command)](#fluxo-write-side-command)
  - [Fluxo Read Side (Query)](#fluxo-read-side-query)
  - [Fluxo Completo — Criação de Conta](#fluxo-completo--criação-de-conta)
  - [Fluxo de Transferência](#fluxo-de-transferência)
  - [Rehidratação do Agregado](#rehidratação-do-agregado)
- [Decisões de Design e Trade-offs](#decisões-de-design-e-trade-offs)
- [Stack Tecnológica](#stack-tecnológica)
- [Como Rodar a Aplicação](#como-rodar-a-aplicação)
- [Telas da Aplicação](#telas-da-aplicação)
- [Referências](#referências)

---

## Padrões Arquiteturais

### CQRS (Command Query Responsibility Segregation)

CQRS é o princípio de separar o modelo que **escreve** dados do modelo que **lê** dados. Em vez de um único modelo que faz as duas coisas, você tem:

- **Write Side (Commands):** recebe intenções de mudança (`CreateAccount`, `Deposit`, `Transfer`), executa validações de negócio e persiste o novo estado.
- **Read Side (Queries):** lê dados de uma projeção otimizada para consulta, sem lógica de negócio, retornando DTOs prontos para a UI.

```
                ┌─────────────────────────────────┐
                │           Aplicação              │
                └──────────┬──────────────┬────────┘
                           │              │
               ┌───────────▼──┐    ┌──────▼──────────┐
               │  Commands     │    │    Queries       │
               │  (write side) │    │    (read side)   │
               └───────────┬──┘    └──────┬───────────┘
                           │              │
               ┌───────────▼──┐    ┌──────▼───────────┐
               │  Write DB     │    │   Read DB         │
               │  (fonte de    │    │   (otimizado      │
               │   verdade)    │    │    para consulta) │
               └──────────────┘    └──────────────────-┘
```

**Quando usar CQRS:**

- A carga de leitura e escrita têm perfis muito diferentes (ex: leituras 100x mais frequentes que escritas)
- O modelo de escrita é rico em regras de negócio, mas o modelo de leitura precisa de dados desnormalizados/agregados
- Você precisa escalar leitura e escrita de forma independente
- O domínio é complexo e misturar leitura e escrita no mesmo modelo cria acoplamento e fragilidade

**Quando NÃO usar CQRS:**

- Domínio simples com CRUD básico — a complexidade operacional não compensa
- Equipe pequena sem experiência com o padrão — a curva de aprendizado é real
- Consistência forte (imediata) é obrigatória em todos os fluxos — CQRS favorece consistência eventual

**Benefícios:**
- Cada lado pode evoluir de forma independente
- Queries nunca travam o write side e vice-versa
- Read models podem ser construídos sob medida para cada caso de uso (ex: uma view MongoDB desnormalizada para o dashboard, outra para relatórios)
- Facilita a escala horizontal — o read side pode ter múltiplas réplicas sem afetar o write side

**Trade-offs:**
- Mais código e mais projetos para manter
- Consistência eventual entre write e read — a UI precisa lidar com isso (estado otimista, polling, websockets)
- Debugging mais complexo: um bug pode estar no handler, na projeção ou na mensageria
- Exige infraestrutura adicional (filas, múltiplos bancos)

---

### Event Sourcing

Event Sourcing é a prática de **armazenar o estado de um agregado como uma sequência de eventos imutáveis**, em vez de persistir o estado atual. O estado atual é sempre derivado — reconstruído (rehidratado) aplicando todos os eventos na ordem.

```
 Abordagem tradicional (CRUD):
 ┌─────────────────────────────────────┐
 │ accounts                            │
 │  id  │ balance │ status │ updatedAt │
 │  001 │  300,00 │ Active │ 2026-07-08│
 └─────────────────────────────────────┘
   Estado atual — histórico perdido

 Event Sourcing:
 ┌──────────────────────────────────────────────────────┐
 │ event_store                                          │
 │  v │ type              │ payload                    │
 │  1 │ AccountCreated    │ { name: "João" }           │
 │  2 │ MoneyDeposited    │ { amount: 500 }            │
 │  3 │ MoneyTransferred  │ { amount: 200 }            │  ← transferiu
 │  4 │ MoneyDeposited    │ { amount: 100 }            │  ← depositou depois
 └──────────────────────────────────────────────────────┘
   Balance atual = 0 + 500 - 200 + 100 = 400   ← calculado, nunca armazenado
```

**Quando usar Event Sourcing:**

- Auditoria é um requisito central — financeiro, saúde, jurídico, compliance
- Você precisa reconstruir o estado em um ponto específico do passado (time travel)
- O histórico de mudanças tem valor de negócio por si só (ex: "quando exatamente o saldo ficou negativo?")
- Integração com outros sistemas via eventos — os eventos do write side alimentam naturalmente outros serviços
- Domínios onde o "o que aconteceu" importa tanto quanto "qual é o estado agora"

**Quando NÃO usar Event Sourcing:**

- Dados que mudam com alta frequência e o histórico não importa (ex: posição GPS em tempo real)
- Equipe sem familiaridade com DDD e agregados — sem um modelo de domínio bem definido, os eventos ficam anêmicos e sem significado
- Sistemas onde consistência imediata é mandatória — Event Sourcing favorece consistência eventual

**Benefícios:**
- Histórico completo e imutável — qualquer estado passado pode ser reconstruído
- Auditoria nativa — sem triggers, sem tabelas de log separadas
- Possibilidade de adicionar novas projeções/read models a partir dos eventos já existentes (event replay)
- Integração desacoplada — outros serviços podem consumir os mesmos eventos
- Debugging poderoso — você pode reproduzir exatamente a sequência de eventos que causou um bug
- Elimina problemas de impedance mismatch entre objeto e banco — eventos são simples DTOs serializáveis

**Trade-offs:**
- Rehidratação pode ser lenta para agregados com histórico longo (solução: snapshots)
- Sem snapshots, toda operação de escrita relê todo o histórico do agregado
- Não existe UPDATE ou DELETE — corrigir um evento errado exige um evento compensatório
- Versioning de eventos é um problema real: quando o schema de um evento muda, eventos antigos precisam de migração ou upcasting
- Consultas ad-hoc no Event Store são difíceis — "qual era o saldo médio de todos os usuários em março?" não é trivial

---

### CQRS + Event Sourcing juntos

Os dois padrões se complementam naturalmente, mas são independentes — você pode ter CQRS sem Event Sourcing e vice-versa. Quando combinados:

- O **Event Store é o write side** — a fonte de verdade imutável
- Os **Read Models são projeções** construídas a partir dos eventos, otimizadas para cada caso de uso
- A **mensageria** (RabbitMQ, Kafka) conecta os dois lados de forma assíncrona

```
 Command → Handler → Event Store (SQL) → Mensageria → Worker → Read Model (MongoDB)
                                            ▲
                                    evento publicado
                                    uma vez, consumido
                                    por N projeções
```

Essa combinação é especialmente poderosa em sistemas distribuídos, event-driven e com múltiplos bounded contexts — cada um podendo ter seu próprio read model projetado a partir dos mesmos eventos de domínio.

---

## Visão Geral

O EWallet é uma carteira digital que permite:

| Operação | Command | Evento(s) gerado(s) |
|---|---|---|
| Criar conta | `CreateAccountCommand` | `AccountCreated` |
| Depositar | `DepositCommand` | `MoneyDeposited` |
| Transferir | `TransferCommand` | `MoneyTransferred` + `MoneyReceived` |
| Reembolsar | `RefundCommand` | `MoneyRefunded` + `MoneyRefundReceived` |
| Bloquear conta | `BlockAccountCommand` | `AccountBlocked` |

Nenhum estado é atualizado diretamente — toda mudança é registrada como um evento imutável no **Event Store** (SQL Server). O estado atual de uma conta é sempre reconstruído (rehidratado) a partir do histórico de eventos.

---

## Arquitetura

### Diagrama de Projetos

```
┌─────────────────────────────────────────────────────────────────┐
│                        EWallet.Blazor                           │
│           (UI — Blazor Server, páginas interativas)             │
│  - Envia Commands via MediatR                                   │
│  - Lê dados via Queries (Read Model / MongoDB)                  │
└──────────────┬──────────────────────────┬───────────────────────┘
               │ Commands/Queries          │ DbContext (Event Store viewer)
               ▼                          ▼
┌─────────────────────────────┐  ┌────────────────────────────────┐
│    EWallet.Application      │  │                                │
│  Commands/                  │  │     EWallet.Infrastructure     │
│    CreateAccountHandler      │  │                                │
│    DepositHandler            │  │  - EWalletDbContext (EF Core)  │
│    TransferHandler           │  │  - EventStoreRepository        │
│    RefundHandler             │  │  - AccountReadRepository       │
│    BlockAccountHandler       │  │  - RabbitMqEventPublisher      │
│  Queries/                   │  │  - RabbitMqConnection          │
│    GetAllAccountsHandler     │  │  - MongoServiceExtensions      │
│    GetAccountByIdHandler     │  │                                │
│    GetEventsByAccountHandler │  └──────────┬─────────────────────┘
│    GetReceivedTransfersHandler│            │
└────────────┬────────────────┘            │
             │ depende de                  │ depende de
             ▼                            ▼
     ┌───────────────────────────────────────────────┐
     │                EWallet.Domain                  │
     │  Entities: Account, AccountReadModel,          │
     │            EventRecord, DomainEvent            │
     │  Events: AccountCreated, MoneyDeposited,       │
     │          MoneyTransferred, MoneyReceived,      │
     │          MoneyRefunded, MoneyRefundReceived,   │
     │          AccountBlocked                        │
     │  Helpers: EventDeserializer                    │
     │  Repositories (interfaces): IEventStoreRepo,  │
     │                             IAccountReadRepo   │
     └───────────────────────────────────────────────┘

┌─────────────────────────────────────────────────────────────────┐
│                       EWallet.Worker                            │
│  (Background Service — consome RabbitMQ e projeta Read Model)   │
│  ReadModelConsumer → despacha para Projections:                 │
│    AccountCreatedProjection                                     │
│    MoneyDepositedProjection                                     │
│    MoneyTransferredProjection                                   │
│    MoneyReceivedProjection                                      │
│    MoneyRefundedProjection                                      │
│    MoneyRefundReceivedProjection                                │
│    AccountBlockedProjection                                     │
└─────────────────────────────────────────────────────────────────┘
```

---

### Fluxo Write Side (Command)

```
 UI (Blazor)
     │
     │  IMediator.Send(XyzCommand)
     ▼
 MediatR Dispatcher
     │
     ▼
 Command Handler
     │
     ├─1─► EventStoreRepository.GetEventsAsync()
     │         └─► SQL Server: SELECT * FROM EventStore WHERE EntityId = @id
     │
     ├─2─► Account.Rehydrate(records)        ← reconstrói estado do zero
     │         └─► foreach event → Apply(event)
     │
     ├─3─► account.Deposit() / Transfer() / etc.
     │         └─► validações de domínio
     │
     ├─4─► EventStoreRepository.SaveEventAsync(event, expectedVersion)
     │         ├─► controle de concorrência (optimistic locking)
     │         └─► SQL Server: INSERT INTO EventStore
     │
     └─5─► IEventPublisher.PublishAsync(event)
               └─► RabbitMQ: exchange "ewallet.events" (fanout)
```

---

### Fluxo Read Side (Query)

```
 UI (Blazor)
     │
     │  IMediator.Send(GetAllAccountsQuery)
     ▼
 MediatR Dispatcher
     │
     ▼
 Query Handler
     │
     └─► IAccountReadRepository.GetAllAsync()
               └─► MongoDB: db.Accounts.find({})
                       └─► AccountReadModel[]
```

---

### Fluxo Completo — Criação de Conta

```
┌──────────┐   CreateAccountCommand   ┌────────────────────┐
│  Blazor  │ ─────────────────────── ▶│ CreateAccountHandler│
└──────────┘                          └────────┬───────────┘
                                               │
                                    ┌──────────▼──────────┐
                                    │  Account.CreateAccount│
                                    │  (novo agregado)      │
                                    └──────────┬──────────-┘
                                               │
                          ┌────────────────────▼──────────────────────┐
                          │  EventStoreRepository.SaveEventAsync()     │
                          │  AccountCreated → SQL Server (EventStore)  │
                          └────────────────────┬──────────────────────┘
                                               │
                          ┌────────────────────▼──────────────────────┐
                          │  RabbitMqEventPublisher.PublishAsync()     │
                          │  Exchange: ewallet.events (fanout)         │
                          └────────────────────┬──────────────────────┘
                                               │
                          ┌────────────────────▼──────────────────────┐
                          │  EWallet.Worker / ReadModelConsumer        │
                          │  Consome fila: ewallet.readmodel           │
                          └────────────────────┬──────────────────────┘
                                               │
                          ┌────────────────────▼──────────────────────┐
                          │  AccountCreatedProjection.ProjectAsync()   │
                          │  AccountReadRepository.UpsertAsync()       │
                          │  MongoDB: db.Accounts.replaceOne(...)      │
                          └───────────────────────────────────────────┘
```

---

### Fluxo de Transferência

Uma transferência gera **dois eventos** — um para cada conta envolvida:

```
TransferCommand { SourceId, DestinationId, Amount }
        │
        ▼
TransferHandler
        │
        ├─► Rehydrate(source)      ← todos os eventos da conta origem
        ├─► Rehydrate(destination) ← todos os eventos da conta destino
        │
        ├─► source.Transfer(destination, amount) ← validações de domínio
        │
        ├─► SaveEvent(sourceId,      MoneyTransferred)  → SQL Server
        ├─► SaveEvent(destinationId, MoneyReceived)      → SQL Server
        │
        ├─► Publish(MoneyTransferred)  → RabbitMQ
        └─► Publish(MoneyReceived)     → RabbitMQ

                    ▼ Worker consome

 MoneyTransferredProjection → source.Balance -= amount    → MongoDB
 MoneyReceivedProjection    → destination.Balance += amount → MongoDB
```

---

### Rehidratação do Agregado

O estado atual de uma conta **nunca é lido de uma tabela de estado**. Ele é sempre reconstruído a partir do Event Store:

```
EventStore (SQL Server)
  ┌────────────────────────────────────────────┐
  │ V1 │ AccountCreated    │ { name: "João" }  │
  │ V2 │ MoneyDeposited    │ { amount: 500 }   │
  │ V3 │ MoneyTransferred  │ { amount: 200 }   │
  │ V4 │ MoneyDeposited    │ { amount: 100 }   │
  └────────────────────────────────────────────┘
               │
               ▼  Account.Rehydrate(records)
               
  foreach record:
    event = EventDeserializer.Deserialize(record)
    account.Apply(event)
    account.Version = record.Version

  Estado final: Balance = 400, Status = Active, Version = 4
```

O `EventDeserializer` resolve o tipo concreto do evento por nome usando reflection com cache estático, garantindo desempenho sem registros manuais.

---

## Decisões de Design e Trade-offs

### Controle de concorrência (Optimistic Locking)

O `EventStoreRepository` verifica a versão esperada antes de salvar:

```
lastVersion = SELECT MAX(Version) FROM EventStore WHERE EntityId = @id
if (lastVersion != expectedVersion)
    throw ConcurrencyException
```

Há também um índice único em `(EntityId, Version)` no banco para garantir no nível do banco de dados.

**Trade-off:** Em cenários de alta concorrência sobre a mesma conta, haverá falhas que exigem retry na camada de aplicação. Para este domínio, isso é aceitável.

---

### RabbitMQ com exchange Fanout

**Decisão:** Todos os eventos são publicados em um único exchange `ewallet.events` do tipo fanout. O Worker tem sua própria fila `ewallet.readmodel` ligada a esse exchange.

**Benefício:** Fácil adicionar novos consumers (ex: notificações, analytics) sem alterar o publisher.

**Trade-off:** Um exchange fanout entrega para todas as filas ligadas, independente do tipo de evento. O `ReadModelConsumer` recebe todos os eventos e descarta os que não têm projeção mapeada com um `LogWarning`. Uma alternativa seria usar um exchange `topic` com routing keys por tipo de evento, mas aumentaria a complexidade de configuração.

---

### Projeções como Strategy Pattern

Cada tipo de evento tem uma projeção dedicada (`IEventProjection`) registrada como Singleton no Worker. O `ReadModelConsumer` mantém um `Dictionary<string, IEventProjection>` e despacha por `EventType`.

**Benefício:** Adicionar suporte a um novo evento é só criar uma nova classe de projeção e registrá-la no DI — nenhum switch gigante para manter.

---

### Rehidratação e o `GetReceivedTransfersQuery`

A query de transferências recebidas não usa o Read Model do MongoDB. Ela relê o Event Store (SQL Server) e filtra apenas eventos do tipo `MoneyReceived`. Isso garante que mesmo que o Read Model ainda não tenha sido projetado, as transferências disponíveis para reembolso sejam sempre precisas — evitando reembolsos duplicados ou indevidos, que são validados no domínio.

---

### Reembolso — Validações de domínio fortes

O agregado `Account` mantém em memória (reconstruída via rehidratação) a lista de transferências recebidas (`_transferenciasRecebidas`) e os eventos já reembolsados (`_eventosReembolsados`). O método `Refund` valida:

- A transferência original existe neste histórico
- A conta de destino é a mesma que enviou a transferência original
- O valor do reembolso é exatamente o valor recebido (sem reembolso parcial)
- A transferência não foi reembolsada anteriormente

Tudo isso sem consultar banco — só a partir dos eventos persistidos.

---

### Blazor Server com atualização de estado otimista

Após cada comando bem-sucedido, a UI atualiza o estado local imediatamente (ex: `conta.Balance -= valor`) sem esperar o Worker processar o evento. Isso elimina a percepção de latência para o usuário em um cenário de consistência eventual.

---

## Stack Tecnológica

| Componente | Tecnologia |
|---|---|
| Framework | .NET 8 |
| UI | Blazor Server (Interactive Server Components) |
| CQRS Dispatcher | MediatR 14 |
| Event Store (Write DB) | SQL Server via EF Core 8 |
| Read Model (Read DB) | MongoDB |
| Mensageria | RabbitMQ (RabbitMQ.Client 6.8) |
| Worker | .NET Worker Service (`IHostedService`) |
| Serialização | System.Text.Json |
| ORM | Entity Framework Core 8 |

---

## Como Rodar a Aplicação

### Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server](https://www.microsoft.com/sql-server) (local na porta 1433) ou Docker
- [MongoDB](https://www.mongodb.com/try/download/community) (local na porta 27017) ou Docker
- [RabbitMQ](https://www.rabbitmq.com/download.html) (local na porta 5672) ou Docker

### 1. Subir as dependências com Docker (recomendado)

Na pasta **pai** deste projeto (`Arquiteturas/`) já existem um `docker-compose.yml` e um script `start-dev.ps1` prontos para subir SQL Server, MongoDB e RabbitMQ com um único comando.

```powershell
# A partir da pasta Arquiteturas/
.\start-dev.ps1
```

O script sobe os containers em background, aguarda 10 segundos e exibe o status e as connection strings. Você pode passar um tempo de espera diferente se precisar:

```powershell
.\start-dev.ps1 -WaitSeconds 20
```

Os containers criados são:

| Container | Imagem | Porta(s) |
|---|---|---|
| `ewallet-sqlserver` | `mssql/server:2022-latest` | `1433` |
| `ewallet-mongo` | `mongo:6.0` | `27017` |
| `ewallet-rabbitmq` | `rabbitmq:3-management` | `5672`, `15672` |

RabbitMQ Management UI: `http://localhost:15672` (user: `guest` / senha: `guest`).

Para parar os containers:

```powershell
# A partir da pasta Arquiteturas/
docker compose down
```

### 2. Configurar as connection strings

Os arquivos `appsettings.json` já estão configurados com os valores padrão acima. Verifique em cada projeto se necessário:

**EWallet.Blazor/appsettings.json** e **EWallet.Worker/appsettings.json:**

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=EWalletDb;User Id=sa;Password=Your_password123;TrustServerCertificate=True;"
  },
  "RabbitMq": {
    "HostName": "localhost",
    "Port": 5672,
    "UserName": "guest",
    "Password": "guest"
  },
  "Mongo": {
    "ConnectionString": "mongodb://localhost:27017",
    "DatabaseName": "EWalletRead",
    "AccountsCollectionName": "Accounts"
  }
}
```

### 3. Aplicar as migrations (criar o Event Store no SQL Server)

Na raiz da solution ou dentro de `EWallet.Blazor`:

```bash
dotnet ef database update --project EWallet.Infrastructure --startup-project EWallet.Blazor
```

Isso criará a tabela `EventStore` com o índice único em `(EntityId, Version)`.

### 4. Rodar os projetos

Abra **dois terminais** e rode em paralelo:

**Terminal 1 — Worker (projeta o Read Model):**
```bash
cd "EWallet.Worker"
dotnet run
```

**Terminal 2 — Blazor (UI + Command Handlers):**
```bash
cd "EWallet.Blazor"
dotnet run
```

A UI estará disponível em `https://localhost:5001` (ou a porta indicada no terminal).

---

## Referências

- [Event Sourcing pattern — Azure Architecture Center (Microsoft)](https://learn.microsoft.com/pt-br/azure/architecture/patterns/event-sourcing)
- [Estilo de arquitetura orientada a eventos — Azure Architecture Center (Microsoft)](https://learn.microsoft.com/pt-br/azure/architecture/guide/architecture-styles/event-driven)
- [Processamento de mensagens idempotentes — AKS Mission Critical Data Platform (Microsoft)](https://learn.microsoft.com/pt-br/azure/architecture/reference-architectures/containers/aks-mission-critical/mission-critical-data-platform#idempotent-message-processing)
- [Building your first Event Store from scratch — Bhargav Koya (Medium)](https://medium.com/@bhargavkoya56/building-your-first-event-store-from-scratch-a-developers-journey-into-c-event-sourcing-640a5f16751b)
- [EventSourcing.NetCore — Projeções e exemplos práticos (Oskar Dudycz, GitHub)](https://github.com/oskardudycz/EventSourcing.NetCore#123-projections)
