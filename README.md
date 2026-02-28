# 🌱 AgroSolutions

## MVP -- Plataforma de Agricultura de Precisão

**Arquitetura de Microsserviços com .NET 8**

------------------------------------------------------------------------

## 📘 Contexto do Projeto

O **AgroSolutions** é um MVP desenvolvido como proposta de solução para
Agricultura de Precisão, utilizando arquitetura baseada em
microsserviços.

O objetivo do projeto é demonstrar:

-   Separação por domínios
-   Comunicação síncrona e assíncrona
-   Banco de dados por serviço
-   Monitoramento e observabilidade
-   Deploy containerizado
-   Orquestração com Kubernetes

A solução foi construída utilizando **.NET 8**, Docker e Kubernetes
(kind).

------------------------------------------------------------------------

## 🏗️ Arquitetura da Solução

A aplicação segue o padrão **Microservices Architecture**, com API
Gateway como ponto único de entrada.

### 🔹 Microsserviços

  -----------------------------------------------------------------------
  Serviço              Porta            Responsabilidade
  -------------------- ---------------- ---------------------------------
  API Gateway (Ocelot) 5000             Roteamento e centralização de
                                        acesso

  Identity Service     5001             Autenticação e emissão de JWT

  Properties Service   5002             Cadastro de propriedades e
                                        talhões

  Ingestion Service    5003             Simulação de dados de sensores

  Analysis Service     5004             Processamento de eventos e
                                        geração de alertas
  -----------------------------------------------------------------------

------------------------------------------------------------------------

## 🗄️ Persistência de Dados

A solução utiliza o conceito de **Database per Service**, garantindo
isolamento entre domínios:

  Serviço      Banco
  ------------ ------------
  Identity     PostgreSQL
  Properties   PostgreSQL
  Ingestion    MongoDB
  Analysis     MongoDB

------------------------------------------------------------------------

## 🔄 Comunicação Entre Serviços

### Comunicação Síncrona

-   HTTP REST via API Gateway

### Comunicação Assíncrona

-   RabbitMQ
-   Ingestion publica eventos
-   Analysis consome eventos

Padrão aplicado: **Event-Driven Architecture**

------------------------------------------------------------------------

## 📊 Observabilidade

A solução implementa monitoramento com:

-   **Prometheus** -- Coleta de métricas
-   **Grafana** -- Visualização e dashboards

Serviços monitorados: - identity-service - properties-service -
ingestion-service - analysis-service

------------------------------------------------------------------------

## 🐳 Execução Local (Docker Compose)

Para subir toda a infraestrutura:

docker compose up --build

Infraestrutura disponível:

  Componente                Porta
  ------------------------- ------------
  PostgreSQL (Identity)     5432
  PostgreSQL (Properties)   5433
  MongoDB                   27017
  RabbitMQ                  15672 (UI)
  Prometheus                9090
  Grafana                   3000

------------------------------------------------------------------------

## ☸️ Execução no Kubernetes (kind)

Acesso ao API Gateway: kubectl -n agro port-forward svc/api-gateway
5000:8080

Acesso ao Grafana: kubectl -n agro port-forward svc/grafana 3000:3000

Usuário padrão: admin\
Senha: admin123

Acesso ao Prometheus: kubectl -n agro port-forward svc/prometheus
9090:9090

Acesso ao RabbitMQ UI: kubectl -n agro port-forward svc/rabbitmq
15672:15672

------------------------------------------------------------------------

## 🧠 Decisões Arquiteturais

-   Arquitetura baseada em Microsserviços\
-   API Gateway com Ocelot\
-   Banco de dados por domínio\
-   Comunicação assíncrona com RabbitMQ\
-   Monitoramento com Prometheus e Grafana\
-   Containerização com Docker\
-   Orquestração com Kubernetes

------------------------------------------------------------------------

## 📌 Tecnologias Utilizadas

-   .NET 8
-   ASP.NET Core
-   Ocelot
-   PostgreSQL
-   MongoDB
-   RabbitMQ
-   Docker
-   Kubernetes (kind)
-   Prometheus
-   Grafana

------------------------------------------------------------------------

## 👨‍🎓 Integrante

  Nome                     RM
  ------------------------ --------
  Rafael Ismair Ferreira   364211
