# 🌱 AgroSolutions

## MVP -- Plataforma de Agricultura de Precisão

**Arquitetura de Microsserviços com .NET 8**

------------------------------------------------------------------------

## 📘 Contexto do Projeto

O **AgroSolutions** é um MVP desenvolvido como proposta de solução para Agricultura de Precisão, utilizando arquitetura baseada em microsserviços.

O objetivo do projeto é demonstrar:

- Separação por domínios (Bounded Contexts)
- Comunicação síncrona e assíncrona
- Banco de dados por serviço (Database per Service)
- Monitoramento e observabilidade
- Deploy containerizado
- Orquestração com Kubernetes

A solução foi construída utilizando **.NET 8**, Docker e Kubernetes (kind).

------------------------------------------------------------------------

## 🏗️ Desenho da Aplicação

### 📐 Visão Arquitetural

```text
                    (Kubernetes / kind)
                    (port-forward 5000 -> 8080)
+------------------------------------------------------+
|                    API Gateway                        |
|                 (Ocelot) :8080                        |
|         Service: api-gateway (externo via PF)         |
+---------------------------+--------------------------+
                            |
        +-------------------+-------------------+-------------------+
        |                   |                   |                   |
        v                   v                   v                   v
+---------------+   +----------------+   +----------------+   +----------------+
| Identity API  |   | Properties API |   | Ingestion API  |   |  Analysis API  |
| svc:80        |   | svc:80         |   | svc:80         |   |  svc:80        |
+-------+-------+   +--------+-------+   +--------+-------+   +--------+-------+
        |                    |                    |                    |
        v                    v                    v                    v
+---------------+   +----------------+      +-------------+      +-------------+
| Postgres      |   | Postgres       |      |  MongoDB     |      |  MongoDB     |
| Identity DB   |   | Properties DB  |      |  Sensors     |      |  Alerts      |
| :5432         |   | :5432          |      |  :27017      |      |  :27017      |
+---------------+   +----------------+      +------+-------+      +------+------+
                                                |                     |
                                                v                     v
                                          +-------------------------------+
                                          |            RabbitMQ            |
                                          |   :5672 (AMQP) / :15672 (UI)   |
                                          +-------------------------------+

Observabilidade
--------------------------------
Prometheus (svc:9090) -> scrape:
- identity-service:80
- properties-service:80
- ingestion-service:80
- analysis-service:80

Grafana (svc:3000) -> dashboards a partir do Prometheus.
```

> Observação: o **API Gateway não expõe Swagger unificado**. Para visualizar Swagger por serviço em Kubernetes, use `port-forward` direto no Service do microsserviço (quando habilitado).

### Fluxo Principal

1. Cliente acessa o **API Gateway**
2. Gateway roteia para o serviço apropriado
3. Ingestion publica eventos no RabbitMQ
4. Analysis consome eventos e gera alertas
5. Métricas são coletadas pelo Prometheus
6. Dashboards são exibidos no Grafana

------------------------------------------------------------------------

## 🏗️ Arquitetura da Solução (DDD + Microservices)

A solução segue **Microservices Architecture** com **API Gateway** como ponto único de entrada.

### 🧩 DDD (Domain-Driven Design)

Aplicamos conceitos de DDD para manter o domínio organizado e reduzir acoplamento entre módulos:

- Cada microsserviço representa um **Bounded Context** (Identity, Properties, Ingestion, Analysis).
- Organização típica por camadas:
  - **Domain**: entidades, regras de negócio, invariantes
  - **Application**: casos de uso (orquestração do fluxo)
  - **Infrastructure**: persistência, mensageria, integrações
  - **API**: controllers/endpoints

### 🔹 Microsserviços

| Serviço | Porta (Docker Compose) | Porta (Kubernetes) | Responsabilidade |
|---|---:|---:|---|
| API Gateway (Ocelot) | 5000 | 8080 (svc) | Roteamento e centralização de acesso |
| Identity Service | 5001 | 80 (svc) | Autenticação e emissão de JWT |
| Properties Service | 5002 | 80 (svc) | Cadastro de propriedades e talhões |
| Ingestion Service | 5003 | 80 (svc) | Simulação/ingestão de dados de sensores |
| Analysis Service | 5004 | 80 (svc) | Processamento de eventos e geração de alertas |

------------------------------------------------------------------------

## 🗄️ Persistência de Dados

A solução utiliza **Database per Service**, garantindo isolamento entre domínios:

| Serviço | Banco |
|---|---|
| Identity | PostgreSQL |
| Properties | PostgreSQL |
| Ingestion | MongoDB |
| Analysis | MongoDB |

------------------------------------------------------------------------

## 🔄 Comunicação Entre Serviços

### Comunicação Síncrona
- HTTP REST via API Gateway

### Comunicação Assíncrona
- RabbitMQ
- Ingestion publica eventos
- Analysis consome eventos

Padrão aplicado: **Event-Driven Architecture**

------------------------------------------------------------------------

## 📊 Observabilidade

A solução implementa monitoramento com:

- **Prometheus** — Coleta de métricas
- **Grafana** — Visualização e dashboards

Serviços monitorados:
- identity-service
- properties-service
- ingestion-service
- analysis-service

------------------------------------------------------------------------

## 🐳 Execução Local (Docker Compose)

Para subir toda a infraestrutura:

```bash
docker compose up --build
```

Infraestrutura disponível:

| Componente | Porta |
|---|---:|
| PostgreSQL (Identity) | 5432 |
| PostgreSQL (Properties) | 5433 |
| MongoDB | 27017 |
| RabbitMQ | 15672 (UI) |
| Prometheus | 9090 |
| Grafana | 3000 |

------------------------------------------------------------------------

## ☸️ Execução no Kubernetes (kind)

### Criar cluster
```powershell
kind create cluster --name agro
kubectl apply -f .\k8s\
kubectl -n agro get pods -w
```

### Acessos (port-forward)

API Gateway:
```powershell
kubectl -n agro port-forward svc/api-gateway 5000:8080
```

Grafana:
```powershell
kubectl -n agro port-forward svc/grafana 3000:3000
```
Usuário padrão: `admin`  
Senha: `admin123`

Prometheus:
```powershell
kubectl -n agro port-forward svc/prometheus 9090:9090
```

RabbitMQ UI:
```powershell
kubectl -n agro port-forward svc/rabbitmq 15672:15672
```

------------------------------------------------------------------------

## 🧠 Decisões Arquiteturais

- Arquitetura baseada em Microsserviços
- API Gateway com Ocelot
- DDD com Bounded Contexts (separação por domínio)
- Banco de dados por domínio (Database per Service)
- Comunicação assíncrona com RabbitMQ (EDA)
- Monitoramento com Prometheus e Grafana
- Containerização com Docker
- Orquestração com Kubernetes (kind)

------------------------------------------------------------------------

## 📌 Tecnologias Utilizadas

- .NET 8
- ASP.NET Core
- Ocelot
- PostgreSQL
- MongoDB
- RabbitMQ
- Docker
- Kubernetes (kind)
- Prometheus
- Grafana

------------------------------------------------------------------------

## 👨‍🎓 Integrante

| Nome | RM |
|---|---:|
| Rafael Ismair Ferreira | 364211 |
