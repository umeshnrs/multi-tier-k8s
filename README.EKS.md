# Multi-Tier Kubernetes Application on AWS EKS (Windows Guide)

A scalable, multi-tier application demonstrating microservices architecture deployed on AWS EKS using Helm charts.

## 📌 Quick Links

### Repository & Images
- **GitHub Repository**: [github.com/umeshnrs/multi-tier-k8s](https://github.com/umeshnrs/multi-tier-k8s)
- **Docker Images**:
  - API: [umesh3149044/api](https://hub.docker.com/r/umesh3149044/api)
  - UI: [umesh3149044/ui](https://hub.docker.com/r/umesh3149044/ui)

### API Endpoints
- Base URL: `http://your-eks-loadbalancer/api`
- Available Endpoints:
  ```
  GET    /api/events       # List all events
  GET    /api/events/{id}  # Get single event
  POST   /api/events       # Create new event
  PUT    /api/events/{id}  # Update event
  DELETE /api/events/{id}  # Delete event
  ```

## 📋 Prerequisites

1. **Required Tools Installation**

   a. **Install Chocolatey (if not installed)**
   ```powershell
   # Run in PowerShell as Administrator
   Set-ExecutionPolicy Bypass -Scope Process -Force; [System.Net.ServicePointManager]::SecurityProtocol = [System.Net.ServicePointManager]::SecurityProtocol -bor 3072; iex ((New-Object System.Net.WebClient).DownloadString('https://community.chocolatey.org/install.ps1'))
   ```

   b. **Install Required Tools**
   ```powershell
   # Run in PowerShell as Administrator
   
   # Install AWS CLI
   choco install awscli -y

   # Install eksctl
   choco install eksctl -y

   # Install kubectl
   choco install kubernetes-cli -y

   # Install Helm
   choco install kubernetes-helm -y
   ```

   c. **Verify Installations**
   ```powershell
   # Open a new PowerShell window and verify installations
   aws --version
   eksctl version
   kubectl version --client
   helm version
   ```

2. **AWS Configuration**
   ```batch
   :: Configure AWS CLI with your credentials
   aws configure
   ```

## 🚀 Deployment Guide

### 1. Create EKS Cluster with EBS CSI Driver

```batch
:: Create EKS cluster with node group for applications
eksctl create cluster `
--name eks-demo-cluster `
--region ap-south-1 `
--version 1.28 `
--with-oidc

:: Create a dedicated node group for applications
eksctl create nodegroup `
--cluster eks-demo-cluster `
--region ap-south-1 `
--name app-nodes `
--node-type t3.medium `
--nodes 2 `
--nodes-min 2 `
--nodes-max 3 `
--managed `
--asg-access

:: Update kubeconfig
aws eks update-kubeconfig --name event-booking-demo --region ap-south-1

:: Create namespace first
kubectl create namespace multi-tier-k8s

:: Set default namespace
kubectl config set-context --current --namespace=multi-tier-k8s

:: Create IAM role and service account for EBS CSI Driver
eksctl create iamserviceaccount `
--name ebs-csi-controller-sa `
--namespace multi-tier-k8s `
--cluster eks-demo-cluster `
--attach-policy-arn arn:aws:iam::aws:policy/service-role/AmazonEBSCSIDriverPolicy `
--approve `
--region ap-south-1

:: Get AWS Account ID
$AWS_ACCOUNT_ID = $(aws sts get-caller-identity --query Account --output text)

:: Install EBS CSI Driver addon
eksctl create addon `
--name aws-ebs-csi-driver `
--cluster eks-demo-cluster `
--service-account-role-arn "arn:aws:iam::${AWS_ACCOUNT_ID}:role/AmazonEKS_EBS_CSI_DriverRole" `
--force `
--region ap-south-1

:: Wait for the EBS CSI driver to be ready (should show multiple pods running)
kubectl get pods -l app.kubernetes.io/name=aws-ebs-csi-driver -w

:: Verify cluster and nodes
kubectl get nodes
```

### 2. Setup Ingress Controller

```batch
:: Add the ingress-nginx repository
helm repo add ingress-nginx https://kubernetes.github.io/ingress-nginx

:: Update helm repositories
helm repo update

:: Install ingress-nginx
helm install ingress-nginx ingress-nginx/ingress-nginx --namespace ingress-nginx --create-namespace

:: Wait for Load Balancer to be ready
kubectl get svc -n ingress-nginx
```

### 3. Deploy Application Components

```batch
# Namespace and context are already set from previous steps

:: Create database credentials
kubectl create secret generic db-credentials --from-literal=password=your-secure-password

:: Deploy PostgreSQL with minimal storage for demo
helm install postgres ./charts/postgres --set persistence.size=1Gi

:: Deploy API
helm install api ./charts/api `
--set replicaCount=2 `
--set config.AllowedOrigins__0=http://your-domain.com

:: Deploy UI
helm install ui ./charts/ui `
--set ingress.host=your-domain.com
```

### 5. Verify Deployment

```batch
:: Check all resources
kubectl get pods,svc,pvc,ingress

:: Verify EBS volume creation
kubectl get pv,pvc

:: Check if PostgreSQL is using the EBS volume
kubectl describe pod -l app=postgres

:: Get Load Balancer URL
kubectl get ingress

:: Check pod logs if needed
kubectl logs -l app=api
kubectl logs -l app=ui

:: Test database connectivity
kubectl exec -it $(kubectl get pod -l app=postgres -o jsonpath="{.items[0].metadata.name}") -- psql -U postgres -d eventbooking -c "\l"
```

## 🔍 Testing & Validation

### Resilience Testing

1. **API Pod Failure Recovery**
   ```batch
   :: Delete an API pod
   kubectl delete pod -l app=api

   :: Watch recreation
   kubectl get pods -l app=api -w
   ```

2. **Database Persistence**
   ```batch
   :: Delete database pod
   kubectl delete pod -l app=postgres

   :: Verify data persistence
   kubectl get pods -l app=postgres -w
   ```

### Troubleshooting Guide

1. **Pod Issues**
   ```batch
   :: Check pod details
   kubectl describe pod [pod-name]

   :: Check logs
   kubectl logs -l app=api
   ```

2. **Service Issues**
   ```batch
   :: Check service details
   kubectl describe svc [service-name]

   :: Check endpoints
   kubectl get endpoints
   ```

3. **Ingress Issues**
   ```batch
   :: Check ingress status
   kubectl describe ingress ui-ingress

   :: Check ingress controller logs
   kubectl logs -n ingress-nginx -l app.kubernetes.io/name=ingress-nginx
   ```

## 🧹 Cleanup

```batch
:: Delete application components
helm uninstall ui api postgres

:: Delete ingress controller
helm uninstall ingress-nginx -n ingress-nginx

:: Delete the EKS cluster
eksctl delete cluster --name eks-demo-cluster --region ap-south-1
```

## 📝 Production Considerations

1. **Security**
   - Enable AWS WAF for the Load Balancer
   - Use AWS Secrets Manager for credentials
   - Implement SSL/TLS with ACM
   - Enable EKS control plane logging

2. **Monitoring**
   - Set up CloudWatch Container Insights
   - Deploy Prometheus and Grafana
   - Configure alerts for critical metrics

3. **Backup**
   - Enable automated EBS snapshots
   - Configure PostgreSQL backups to S3
   - Implement disaster recovery plan

4. **Cost Optimization**
   - Use Spot Instances for non-critical workloads
   - Implement cluster autoscaling
   - Monitor and optimize resource requests/limits