# Multi-Tier Application Deployment

## Prerequisites

- Docker Desktop with Kubernetes enabled
- Helm v3.x
- PowerShell (Windows) or Terminal (Linux/Mac)

## Quick Start

### 1. Docker Compose Setup

```bash
# Build and run with Docker Compose
docker-compose build
docker-compose up -d

# Check services
docker-compose ps

# Stop services
docker-compose down
```

#### Docker Compose Troubleshooting

1. Service Startup Issues
   - Verify Docker Desktop is running
   - Check if required ports are available:
     * API: 5000
     * UI: 5173
     * PostgreSQL: 5432
   - Run `docker-compose ps` to check service status
   - View logs with `docker-compose logs <service-name>`

2. Database Issues
   - Check PostgreSQL container status: `docker-compose ps postgres`
   - View database logs: `docker-compose logs postgres`
   - Verify database connection string in `appsettings.json`
   - Try recreating the volume: 
     ```bash
     docker-compose down -v
     docker-compose up -d
     ```

3. Network Issues
   - Check if services are on the same network: `docker network ls`
   - Inspect network: `docker network inspect multi-tier-k8s_default`
   - Verify service names match in docker-compose.yml

### 2. Kubernetes Deployment with Helm

The application is now structured as three separate Helm charts for better modularity and independent deployment:
- PostgreSQL database (`charts/postgres`)
- Backend API (`charts/api`)
- Frontend UI (`charts/ui`)

#### Namespace Setup
```bash
# Create the event-system namespace
kubectl create namespace event-system

# Optional: Set as default namespace for current context
kubectl config set-context --current --namespace=event-system

# Verify namespace
kubectl get namespace event-system
```

#### Create Common Database Secret
```bash
# Create a common database credentials secret
kubectl create secret generic db-credentials --namespace event-system  --from-literal=password=local

# Verify the secret was created
kubectl get secret db-credentials -n event-system
```

This secret will be used by both PostgreSQL and API services for database authentication.

#### Deploy PostgreSQL
```bash
# Install PostgreSQL chart
helm install postgres ./charts/postgres -n event-system

# Verify PostgreSQL deployment
kubectl get pods postgres-0 -n event-system
kubectl logs postgres-0

kubectl get pvc
```

#### Deploy API
```bash
# Install API chart with PostgreSQL reference
helm install api ./charts/api -n event-system 

# Verify API deployment and configuration
kubectl get pods -l app=api
kubectl get configmap
kubectl exec -it $(kubectl get pod -l app=api -o name) -- env | grep DB_
```

The API deployment:
- Uses ConfigMap from Postgres for database configuration
- Builds connection string using environment variables
- References Postgres secret for database password
- Exposes both HTTP (5000) and HTTPS (5001) ports

#### Deploy UI
```bash

helm repo add ingress-nginx https://kubernetes.github.io/ingress-nginx  -n ingress-nginx
helm install ingress-nginx ingress-nginx/ingress-nginx -n event-system

helm uninstall ingress-nginx -n event-system
helm uninstall ingress-nginx -n ingress-nginx

# Install UI chart
helm install ui ./charts/ui -n event-system
```