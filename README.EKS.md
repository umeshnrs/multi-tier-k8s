# AWS EKS Deployment Guide

This guide provides step-by-step instructions for deploying the multi-tier application on Amazon EKS.

## Prerequisites

1. **AWS CLI** - [Install and Configure AWS CLI](https://aws.amazon.com/cli/)
2. **kubectl** - [Install kubectl](https://kubernetes.io/docs/tasks/tools/install-kubectl/)
3. **eksctl** - [Install eksctl](https://eksctl.io/installation/)
4. **Helm v3** - [Install Helm](https://helm.sh/docs/intro/install/)

## EKS Cluster Setup

1. **Create EKS Cluster**

```bash
eksctl create cluster \
  --name eks-test \
  --region ap-south-1 \
  --nodes 2 \
  --node-type t3.medium \
  --managed
```

2. **Configure kubectl**
```bash
aws eks update-kubeconfig --name eks-test --region ap-south-1
```

## Install Required Components

1. **Install AWS EBS CSI Driver**
```bash
# Add the AWS EBS CSI Driver Helm repository
helm repo add aws-ebs-csi-driver https://kubernetes-sigs.github.io/aws-ebs-csi-driver
helm repo update

# Install the AWS EBS CSI Driver
helm install aws-ebs-csi-driver aws-ebs-csi-driver/aws-ebs-csi-driver \
  --namespace kube-system \
  --set controller.serviceAccount.create=true \
  --set controller.serviceAccount.annotations."eks\\.amazonaws\\.com/role-arn"=arn:aws:iam::<accountid>:role/AmazonEKS_EBS_CSI_DriverRole
```

2. **Install NGINX Ingress Controller**
```bash
# Add the ingress-nginx repository
helm repo add ingress-nginx https://kubernetes.github.io/ingress-nginx
helm repo update

# Install the ingress-nginx controller
helm install ingress-nginx ingress-nginx/ingress-nginx \
  --namespace ingress-nginx \
  --create-namespace \
  --set controller.service.type=LoadBalancer

  kubectl describe svc ingress-nginx-controller -n ingress-nginx
  
```

## Application Deployment

1. **Create Namespace**
```bash
kubectl create namespace multi-tier-k8s
kubectl config set-context --current --namespace=multi-tier-k8s
```

2. **Create Database Credentials**
```bash
kubectl create secret generic db-credentials \
  --namespace multi-tier-k8s \
  --from-literal=password=local
```

3. **Deploy Application Components**
```bash
# Deploy PostgreSQL
helm install postgres ./charts/postgres

# Deploy API
helm install api ./charts/api

# Deploy UI
helm install ui ./charts/ui
```

## Verify Deployment

1. **Check Resources**
```bash
# Check all resources
kubectl get all -n multi-tier-k8s

# Check storage resources
kubectl get pv,pvc -n multi-tier-k8s

# Check ingress
kubectl get ingress -n multi-tier-k8s
```

2. **Get Application URL**
```bash
# Get the LoadBalancer URL
export SERVICE_URL=$(kubectl get svc -n ingress-nginx ingress-nginx-controller -o jsonpath='{.status.loadBalancer.ingress[0].hostname}')
echo "Application is accessible at: http://$SERVICE_URL"
```

## Scaling and Updates

1. **Scale API Deployment**
```bash
# Scale API replicas
helm upgrade api ./charts/api --set replicaCount=3
```

2. **Update Components**
```bash
# Update PostgreSQL
helm upgrade postgres ./charts/postgres

# Update API
helm upgrade api ./charts/api

# Update UI
helm upgrade ui ./charts/ui
```

## Monitoring and Logs

1. **View Pod Logs**
```bash
# API logs
kubectl logs -l app=api -n multi-tier-k8s

# UI logs
kubectl logs -l app=ui -n multi-tier-k8s

# Database logs
kubectl logs -l app=postgres -n multi-tier-k8s
```

2. **Check Pod Status**
```bash
kubectl describe pods -n multi-tier-k8s
```

## Cleanup

1. **Remove Application**
```bash
# Uninstall application components
helm uninstall ui api postgres -n multi-tier-k8s

# Delete namespace
kubectl delete namespace multi-tier-k8s
```

2. **Remove Infrastructure**
```bash
# Remove ingress controller
helm uninstall ingress-nginx -n ingress-nginx
kubectl delete namespace ingress-nginx

# Remove EBS CSI Driver
helm uninstall aws-ebs-csi-driver -n kube-system

# Delete EKS cluster
eksctl delete cluster --name eks-test --region ap-south-1
```

## Important Notes

1. **Storage**:
   - The application uses AWS EBS volumes for PostgreSQL persistence
   - Default storage size is 1Gi for demo purposes
   - Storage class uses gp3 volume type for better performance/cost ratio

2. **Security**:
   - Update the database password in production
   - Consider implementing SSL/TLS for ingress
   - Review and adjust IAM roles and policies

3. **Production Considerations**:
   - Implement proper monitoring and alerting
   - Set up regular database backups
   - Configure horizontal pod autoscaling
   - Use node affinity and pod anti-affinity for high availability
   - Consider using AWS Secrets Manager for sensitive data

4. **Cost Optimization**:
   - The demo uses minimal resources for testing
   - Adjust resource requests/limits based on actual needs
   - Consider using Spot Instances for non-critical workloads
   - Monitor EBS volume usage and adjust size as needed
