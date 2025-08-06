# Multi-Tier Kubernetes Application

A scalable, multi-tier application demonstrating microservices architecture deployed on Kubernetes using Helm charts.

## 📌 Quick Links

### Documentation & Resources
- **DevOps Documentation**: [DEVOPS.md](./DEVOPS.md) - Detailed infrastructure and deployment documentation
- **Demo Video**: [Watch Demo](https://drive.google.com/file/d/17W6R96Pg9bAPULwBtPOQREKNJkadasF9/view?usp=sharing)

### Repository & Images
- **GitHub Repository**: [github.com/umeshnrs/multi-tier-k8s](https://github.com/umeshnrs/multi-tier-k8s)
- **Docker Images**:
  - API: [umesh3149044/api](https://hub.docker.com/r/umesh3149044/api)
  - UI: [umesh3149044/ui](https://hub.docker.com/r/umesh3149044/ui)

### API Endpoints
- Base URL: `http://localhost/api`
- Available Endpoints:
  ```
  GET    /api/events       # List all events
  GET    /api/events/{id}  # Get single event
  POST   /api/events       # Create new event
  PUT    /api/events/{id}  # Update event
  DELETE /api/events/{id}  # Delete event
  ```

## 📋 Project Documentation

### Requirements Analysis
- **Core Requirements**
  - Multi-tier application architecture (Frontend, Backend, Database)
  - Containerized components with Docker
  - Kubernetes deployment with high availability
  - Data persistence across pod failures
  - Service discovery and load balancing
  - Easy scaling and updates
  - Comprehensive monitoring and logging

### Technical Architecture

#### Component Stack
- **Frontend**: Vue.js + TypeScript + Nginx
  - Single Page Application (SPA)
  - Responsive design
  - TypeScript for type safety
  - Nginx for static file serving

- **Backend**: .NET Core Web API
  - RESTful API design
  - Entity Framework Core
  - CQRS pattern with MediatR
  - Swagger documentation

- **Database**: PostgreSQL
  - Persistent volume storage
  - Automated backups
  - Data integrity constraints

#### Infrastructure
- **Container Orchestration**: Kubernetes
  - Helm charts for deployment
  - Ingress-Nginx for routing
  - ConfigMaps and Secrets management
  - Health checks and probes

### Resource Specifications

#### Kubernetes Resources
```yaml
API Service:
  CPU: 0.2-1.0 cores
  Memory: 256Mi-512Mi
  Replicas: 2-5

UI Service:
  CPU: 0.1-0.5 cores
  Memory: 128Mi-256Mi
  Replicas: 2-3

Database:
  CPU: 0.5-1.0 cores
  Memory: 512Mi-1Gi
  Storage: 0.5Gi
```

## 🚀 Deployment Guide

### Prerequisites
- Docker Desktop with Kubernetes enabled
- Helm v3.x
- PowerShell (Windows) or Terminal (Linux/Mac)

### Local Development Setup

```bash
# Build and run with Docker Compose
docker-compose build
docker-compose up -d

# Verify services
docker-compose ps
```

### Kubernetes Deployment

1. **Create Namespace and Setup**
   ```bash
   kubectl create namespace multi-tier-k8s
   kubectl config set-context --current --namespace=multi-tier-k8s
   ```

2. **Install Ingress Controller**
   ```bash
   helm repo add ingress-nginx https://kubernetes.github.io/ingress-nginx
   helm repo update
   helm install ingress-nginx ingress-nginx/ingress-nginx -n ingress-nginx --create-namespace
   ```

3. **Deploy Application Components**
   ```bash
   # Create database credentials
   kubectl create secret generic db-credentials -n multi-tier-k8s --from-literal=password=local

   # Deploy all components
   helm install postgres ./charts/postgres -n multi-tier-k8s
   helm install api ./charts/api -n multi-tier-k8s
   helm install ui ./charts/ui -n multi-tier-k8s
   ```

4. **Upgrade Application Components**
   ```bash
   # Update to a new version[any component]
   helm upgrade postgres ./charts/postgres -n multi-tier-k8s
   helm upgrade api ./charts/api -n multi-tier-k8s
   helm upgrade ui ./charts/ui -n multi-tier-k8s
   ```

### Verification Steps

1. **Check Deployment Status**
   ```bash
   kubectl get pods,svc,ingress -n multi-tier-k8s
   ```

2. **Access Applications**
   - UI: http://localhost/
   - API: http://localhost/api/events

## 🔍 Testing & Validation

### Resilience Testing
1. **API Pod Failure Recovery**
   ```bash
   # Delete API pod
   kubectl delete pod -l app=api -n multi-tier-k8s
   # Watch automatic recreation
   kubectl get pods -w -l app=api -n multi-tier-k8s
   ```

2. **Database Persistence**
   ```bash
   # Delete database pod
   kubectl delete pod -l app=postgres -n multi-tier-k8s
   # Verify data persistence after pod recreation
   kubectl get pods -w -l app=postgres -n multi-tier-k8s
   ```

3. **Observe Rolling Updates**
   ```bash
   # Watch the rolling update process in real-time
   kubectl get pods -w -l app=api -n multi-tier-k8s

   # In another terminal, trigger an update (e.g., change image version)
   helm upgrade api ./charts/api -n multi-tier-k8s --set image.tag=v1.0.1

   # You should see:
   # 1. New pod being created
   # 2. Once new pod is ready, old pod terminating
   # 3. Process repeats until all pods are updated
   # This ensures zero downtime as only one pod is updated at a time
   ```

   To verify application availability during updates:
   ```bash
   # In another terminal, continuously test the API
   while true; do curl -s -o /dev/null -w "%{http_code}\n" http://localhost/api/events; sleep 1; done
   ```

### Troubleshooting Guide

1. **Pod Issues**
   ```bash
   kubectl describe pod <pod-name> -n multi-tier-k8s
   kubectl logs -n multi-tier-k8s -l app=api
   ```

2. **Service Issues**
   ```bash
   kubectl describe svc <service-name> -n multi-tier-k8s
   kubectl get endpoints -n multi-tier-k8s
   ```

3. **Ingress Issues**
   ```bash
   kubectl describe ingress ui-ingress -n multi-tier-k8s
   kubectl logs -n ingress-nginx -l app.kubernetes.io/name=ingress-nginx
   ```

## 🧹 Cleanup

```bash
# Remove application components
helm uninstall ui api postgres -n multi-tier-k8s

# Remove ingress controller
helm uninstall ingress-nginx -n ingress-nginx

# Delete namespaces
kubectl delete namespace multi-tier-k8s ingress-nginx

# Remove volumes (optional)
kubectl delete pv --all
```

## 📝 Notes
- **Demo Video**: Check out our [demonstration video](https://drive.google.com/file/d/17W6R96Pg9bAPULwBtPOQREKNJkadasF9/view?usp=sharing) showing deployment, API functionality, and resilience testing
- **DevOps Documentation**: For detailed infrastructure, deployment architecture, and operational guides, see [DEVOPS.md](./DEVOPS.md)
- For production deployments, consider implementing:
  - SSL/TLS encryption
  - Regular database backups
  - Monitoring and alerting
  - CI/CD pipelines