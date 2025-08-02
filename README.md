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

### 2. Kubernetes Deployment with Helm

```bash
# Deploy with Helm (in default namespace)
helm install multi-tier ./helm -n default

# Verify deployment
kubectl get pods
kubectl get services

# Access the application
# Add to hosts file: 127.0.0.1 multi-tier.local
# Access at: http://multi-tier.local

# Uninstall
helm uninstall multi-tier
```

### 3. Common Commands

```bash
# View logs
kubectl logs -f deployment/api
kubectl logs -f deployment/ui

# Restart deployments
kubectl rollout restart deployment/api
kubectl rollout restart deployment/ui

# Clean up
helm uninstall multi-tier -n default
docker-compose down -v
```

## Troubleshooting

- If services don't start, check Docker Desktop is running with Kubernetes enabled
- For CORS issues, verify ingress configuration in helm/values.yaml
- For database connection issues, check PostgreSQL service is running
- For port conflicts, ensure ports 5000 (API) and 5173 (UI) are available