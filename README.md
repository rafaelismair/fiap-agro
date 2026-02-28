# AgroSolutions - MVP Agricultura de Precisão (Microservices .NET 8)

## Serviços
- API Gateway (Ocelot) - porta `5000`
- Identity Service (JWT) - porta `5001`
- Properties Service (Propriedades/Talhões) - porta `5002`
- Ingestion Service (sensores simulados) - porta `5003`
- Analysis Service (alertas via RabbitMQ consumer) - porta `5004`

Infra:
- PostgreSQL (identity) `5432`
- PostgreSQL (properties) `5433`
- MongoDB `27017` 
- RabbitMQ UI `15672`
- Prometheus `9090`
- Grafana `3000`

## Rodar local (Docker Compose)
```bash
docker compose up --build
