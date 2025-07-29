# Multi-Tier Application Deployment

## Prerequisites

- Windows 10/11
- Docker Desktop with Kubernetes enabled
- Helm v3.x
- PowerShell

## Windows Setup Commands

### 1. Docker Desktop Setup
```powershell
# Verify Docker installation
docker --version
docker-compose --version

# Verify Kubernetes is running
kubectl version
kubectl config current-context  # Should show 'docker-desktop'
```

### 2. Essential Commands (PowerShell)

1. Namespace Management:
   ```powershell
   # Create and set namespace
   kubectl create namespace multi-tier-app
   kubectl config set-context --current --namespace=multi-tier-app
   
   # Verify namespace
   kubectl config get-contexts
   ```

2. Docker Image Management:
   ```powershell
   # Build images
   docker build -t umesh3149044/api:latest -f .\api\EventBooking.API\Dockerfile .\api
   docker build -t umesh3149044/ui:latest .\ui
   
   # Push to Docker Hub (after docker login)
   docker login
   docker push umesh3149044/api:latest
   docker push umesh3149044/ui:latest
   ```

3. Volume Management:
   ```powershell
   # Create volume
   docker volume create postgres_data_dev
   
   # List volumes
   docker volume list
   ```

4. Host File Configuration:
   ```powershell
   # Run PowerShell as Administrator and add to hosts file
   Add-Content -Path $env:windir\System32\drivers\etc\hosts -Value "`n127.0.0.1 multi-tier.local" -Force
   ```

## Helm Deployment Commands

### 1. Basic Helm Operations
```powershell
# Initialize deployment
helm install multi-tier .\helm

# Check status
helm status multi-tier

# List all releases
helm list

# Upgrade deployment
helm upgrade multi-tier .\helm

# Rollback to previous version
helm rollback multi-tier 1

# Uninstall release
helm uninstall multi-tier
```

### 2. Deploying Individual Components
```powershell
# First verify the deployments exist
kubectl get deployments -n default

# Deploy/Update only the API
kubectl rollout restart deployment/api -n default

# Deploy/Update only the UI
kubectl rollout restart deployment/ui -n default

# Watch the rollout status
kubectl rollout status deployment/api -n default
kubectl rollout status deployment/ui -n default

# Check the logs of new pods
kubectl logs -f deployment/api -n default
kubectl logs -f deployment/ui -n default

# Rollback individual deployment if needed
kubectl rollout undo deployment/api -n default
kubectl rollout undo deployment/ui -n default

# Check deployment status
kubectl get pods -n default -l app=api -w  # For API
kubectl get pods -n default -l app=ui -w   # For UI
```

### 3. Environment Variables and CORS Configuration

1. UI Environment Variables:
   ```yaml
   # In values.yaml
   ui:
     buildArgs:
       VITE_API_URL: "/api"  # Use relative path for API URL
   ```

2. API CORS Configuration:
   ```yaml
   # In values.yaml
   ingress:
     annotations:
       nginx.ingress.kubernetes.io/cors-allow-methods: "GET, PUT, POST, DELETE, PATCH, OPTIONS"
       nginx.ingress.kubernetes.io/cors-allow-origin: "*"
       nginx.ingress.kubernetes.io/cors-allow-credentials: "true"
       nginx.ingress.kubernetes.io/enable-cors: "true"
   ```

3. Nginx CORS Headers:
   ```nginx
   # In nginx.conf
   add_header Access-Control-Allow-Origin $cors_origin always;
   add_header Access-Control-Allow-Methods "GET, PUT, POST, DELETE, PATCH, OPTIONS" always;
   add_header Access-Control-Allow-Headers "*" always;
   ```

### 4. Common Issues and Solutions

1. CORS Issues:
   - Check ingress annotations for CORS configuration
   - Verify API's allowed origins
   - Ensure nginx is configured to handle CORS headers
   - Use browser dev tools to check CORS errors

2. Environment Variables:
   - Check UI's VITE_API_URL setting
   - Verify environment variables in running pods
   - Use relative paths for API URLs

3. HTTPS/HTTP Issues:
   - Control HTTPS redirection in ingress
   - Configure SSL settings appropriately
   - Check browser console for mixed content warnings

4. Database Connection:
   - Verify PostgreSQL service is running
   - Check connection string configuration
   - Ensure database credentials are correct

### 5. Debugging Commands
```powershell
# Check environment variables in pods
kubectl exec -it deployment/ui -n default -- env | findstr VITE
kubectl exec -it deployment/api -n default -- env

# View pod logs
kubectl logs -f deployment/api -n default
kubectl logs -f deployment/ui -n default

# Check nginx configuration
kubectl exec -it deployment/ui -n default -- cat /etc/nginx/conf.d/default.conf

# Test services directly
kubectl port-forward service/api 6000:6000 -n default
kubectl port-forward service/ui 5173:80 -n default
```

## Kubernetes Verification Commands

### 1. Pod Management
```powershell
# Watch pods
kubectl get pods -w

# Get pod logs
kubectl logs -f deployment/api
kubectl logs -f deployment/ui

# Execute into pod
kubectl exec -it <pod-name> -- powershell
```

### 2. Service Verification
```powershell
# Check services
kubectl get services

# Port forwarding
kubectl port-forward service/api 6000:6000
kubectl port-forward service/ui 5173:80

# Test endpoints
Invoke-WebRequest -Uri http://multi-tier.local/api/health
Invoke-WebRequest -Uri http://multi-tier.local
```

### 3. Resource Monitoring
```powershell
# Check resource usage
kubectl top pods
kubectl top nodes

# View events
kubectl get events --sort-by='.lastTimestamp'
```

## Cleanup Commands

### 1. Remove Deployment
```powershell
# Uninstall Helm release
helm uninstall multi-tier

# Delete namespace and resources
kubectl delete namespace multi-tier-app

# Clean up volumes
docker volume rm postgres_data_dev

# Remove images
docker rmi umesh3149044/api:latest
docker rmi umesh3149044/ui:latest
```

### 2. Reset Docker Desktop
```powershell
# Reset Kubernetes cluster
kubectl config use-context docker-desktop
kubectl delete --all pods --namespace=multi-tier-app
kubectl delete --all deployments --namespace=multi-tier-app
kubectl delete --all services --namespace=multi-tier-app
```

## Troubleshooting Windows-Specific Issues

### 1. Docker Desktop Issues
```powershell
# Restart Docker Desktop
Stop-Service com.docker.service
Start-Service com.docker.service

# Reset Kubernetes
kubectl config use-context docker-desktop
```

### 2. Port Conflicts
```powershell
# Check port usage
netstat -ano | findstr "5433"
netstat -ano | findstr "6000"
netstat -ano | findstr "5173"

# Find and stop conflicting process
$port = 5433
$processId = (Get-NetTCPConnection -LocalPort $port).OwningProcess
Stop-Process -Id $processId -Force
```

### 3. WSL Integration
```powershell
# Check WSL status
wsl --status
wsl --list --verbose

# Restart WSL
wsl --shutdown
```

### 4. Common Fixes
```powershell
# Clear Docker cache
docker system prune -af

# Reset Kubernetes config
Remove-Item -Path "$HOME\.kube\config" -Force
docker context use default
```