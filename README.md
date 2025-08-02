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

The application is structured as three separate Helm charts for better modularity and independent deployment:
- PostgreSQL database (`charts/postgres`)
- Backend API (`charts/api`)
- Frontend UI (`charts/ui`)

#### Initial Setup

```bash
# Create namespace
kubectl create namespace event-system

# Optional: Set as default namespace
kubectl config set-context --current --namespace=event-system

# Install Ingress Controller
helm repo add ingress-nginx https://kubernetes.github.io/ingress-nginx
helm repo update
helm install ingress-nginx ingress-nginx/ingress-nginx --namespace ingress-nginx --create-namespace

# Create database credentials
kubectl create secret generic db-credentials --namespace event-system --from-literal=password=local
```

#### Deploy Application

```bash
# Deploy PostgreSQL
helm install postgres ./charts/postgres -n event-system

# Deploy API
helm install api ./charts/api -n event-system

# Deploy UI
helm install ui ./charts/ui -n event-system
```

#### Verify Deployment

```bash
# Check all pods
kubectl get pods -n event-system

# Check services
kubectl get svc -n event-system

# Check ingress
kubectl get ingress -n event-system

# View application logs
kubectl logs -n event-system -l app=api
kubectl logs -n event-system -l app=ui
```

#### Access the Application

The application will be available at:
- UI: http://localhost/
- API: http://localhost/api/events

#### Upgrade Components

```bash
# Upgrade PostgreSQL
helm upgrade postgres ./charts/postgres -n event-system

# Upgrade API
helm upgrade api ./charts/api -n event-system

# Upgrade UI
helm upgrade ui ./charts/ui -n event-system
```

#### Shutdown and Cleanup

```bash
# Delete all application components
helm uninstall ui -n event-system
helm uninstall api -n event-system
helm uninstall postgres -n event-system

# Delete ingress controller
helm uninstall ingress-nginx -n ingress-nginx

# Delete namespace (this will delete everything in it)
kubectl delete namespace event-system
kubectl delete namespace ingress-nginx

# Optional: Remove persistent volumes
kubectl delete pv --all
```

### Troubleshooting

1. Check pod status:
```bash
kubectl get pods -n event-system
kubectl describe pod <pod-name> -n event-system
```

2. View logs:
```bash
kubectl logs -n event-system -l app=api
kubectl logs -n event-system -l app=ui
kubectl logs -n ingress-nginx -l app.kubernetes.io/name=ingress-nginx
```

3. Check ingress:
```bash
kubectl get ingress -n event-system
kubectl describe ingress ui-ingress -n event-system
```

4. Port forward for direct access:
```bash
kubectl port-forward -n event-system svc/api 5000:5000
kubectl port-forward -n event-system svc/ui 8080:80
```