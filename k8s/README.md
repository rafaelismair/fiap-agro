# Kubernetes (kind) - AgroSolutions

Manifests mínimos para rodar a stack no Kubernetes (kind) usando imagens publicadas no Docker Hub.

## Pré-requisitos
- Docker Desktop
- kubectl
- kind

## 1) Criar cluster kind
```powershell
kind create cluster --name agro
kubectl cluster-info --context kind-agro
kubectl get nodes
```

## 2) Aplicar manifests
Na raiz do repo:

```powershell
kubectl apply -f .\k8s\
kubectl -n agro get pods -w
```

## 3) Port-forward (para demo/vídeo)
API Gateway:
```powershell
kubectl -n agro port-forward svc/api-gateway 5000:8080
```

Prometheus:
```powershell
kubectl -n agro port-forward svc/prometheus 9090:9090
```

Grafana:
```powershell
kubectl -n agro port-forward svc/grafana 3000:3000
```

RabbitMQ UI:
```powershell
kubectl -n agro port-forward svc/rabbitmq 15672:15672
```

## 4) Dicas de verificação
```powershell
kubectl -n agro get all
kubectl -n agro describe pod <pod>
kubectl -n agro logs deploy/api-gateway
```

## 5) Remover tudo
```powershell
kubectl delete -f .\k8s\
kind delete cluster --name agro
```