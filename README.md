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
kubectl create namespace multi-tier-k8s

# Optional: Set as default namespace
kubectl config set-context --current --namespace=multi-tier-k8s

# Install Ingress Controller
helm repo add ingress-nginx https://kubernetes.github.io/ingress-nginx
helm repo update
helm install ingress-nginx ingress-nginx/ingress-nginx --namespace ingress-nginx --create-namespace

# Create database credentials
kubectl create secret generic db-credentials --namespace multi-tier-k8s --from-literal=password=local
```

#### Deploy Application

```bash
# Deploy PostgreSQL
helm install postgres ./charts/postgres -n multi-tier-k8s

# Deploy API
helm install api ./charts/api -n multi-tier-k8s

# Deploy UI
helm install ui ./charts/ui -n multi-tier-k8s
```

#### Start All Components at Once

```bash
# Deploy all components in the correct order
helm install postgres ./charts/postgres -n multi-tier-k8s && \
helm install api ./charts/api -n multi-tier-k8s && \
helm install ui ./charts/ui -n multi-tier-k8s
```

#### Verify Deployment

```bash
# Check all pods
kubectl get pods -n multi-tier-k8s

# Check services
kubectl get svc -n multi-tier-k8s

# Check ingress
kubectl get ingress -n multi-tier-k8s

# View application logs
kubectl logs -n multi-tier-k8s -l app=api
kubectl logs -n multi-tier-k8s -l app=ui
```

#### Access the Application

The application will be available at:
- UI: http://localhost/
- API: http://localhost/api/events

#### Upgrade Components

```bash
# Upgrade PostgreSQL
helm upgrade postgres ./charts/postgres -n multi-tier-k8s

# Upgrade API
helm upgrade api ./charts/api -n multi-tier-k8s

# Upgrade UI
helm upgrade ui ./charts/ui -n multi-tier-k8s
```

#### Shutdown and Cleanup

```bash
# Delete all application components
helm uninstall ui -n multi-tier-k8s
helm uninstall api -n multi-tier-k8s
helm uninstall postgres -n multi-tier-k8s

# Delete ingress controller
helm uninstall ingress-nginx -n ingress-nginx

# Delete namespace (this will delete everything in it)
kubectl delete namespace multi-tier-k8s
kubectl delete namespace ingress-nginx

# Optional: Remove persistent volumes
kubectl delete pv --all
```

### Troubleshooting

1. Check pod status:
```bash
kubectl get pods -n multi-tier-k8s
kubectl describe pod <pod-name> -n multi-tier-k8s
```

2. View logs:
```bash
kubectl logs -n multi-tier-k8s -l app=api
kubectl logs -n multi-tier-k8s -l app=ui
kubectl logs -n ingress-nginx -l app.kubernetes.io/name=ingress-nginx
```

3. Check ingress:
```bash
kubectl get ingress -n multi-tier-k8s
kubectl describe ingress ui-ingress -n multi-tier-k8s
```

4. Port forward for direct access:
```bash
kubectl port-forward -n multi-tier-k8s svc/api 5000:5000
kubectl port-forward -n multi-tier-k8s svc/ui 8080:80
```