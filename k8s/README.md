# CleanAuth Kubernetes Local Environment

This directory contains Kubernetes manifests and scripts to run CleanAuth locally using Kubernetes.

## Directory Structure

```
k8s/
├── infrastructure/          # Core infrastructure components
│   ├── namespace.yaml      # Kubernetes namespace
│   ├── secrets.yaml        # Sensitive data (passwords, JWT secret)
│   └── configmap.yaml      # Application configuration
├── database/               # PostgreSQL database components
│   ├── postgres-pvc.yaml   # Persistent Volume Claim
│   ├── postgres-deployment.yaml  # PostgreSQL deployment
│   └── postgres-service.yaml     # PostgreSQL service
├── api/                    # CleanAuth API components
│   ├── api-deployment.yaml # API deployment
│   └── api-service.yaml    # API service (NodePort)
├── scripts/                # Deployment and utility scripts
│   ├── deploy.sh          # Automated deployment
│   └── cleanup.sh         # Environment cleanup
└── README.md              # This documentation
```

## Prerequisites

- **Kubernetes cluster**: minikube, kind, Docker Desktop with Kubernetes, or any local k8s cluster
- **kubectl**: Kubernetes command-line tool
- **Docker**: For building the application image

## Quick Start

1. **Make scripts executable:**
   ```bash
   chmod +x k8s/scripts/deploy.sh k8s/scripts/cleanup.sh
   ```

2. **Deploy the application:**
   ```bash
   ./k8s/scripts/deploy.sh
   ```

3. **Access the application:**
   - API: http://localhost:30001
   - The NodePort service exposes the API on port 30001

## Manual Deployment

If you prefer to deploy manually:

```bash
```bash
# Build the Docker image
docker build -t clean-auth-api:latest .

# Apply Kubernetes manifests
kubectl apply -f k8s/infrastructure/namespace.yaml
kubectl apply -f k8s/infrastructure/secrets.yaml
kubectl apply -f k8s/database/postgres-pvc.yaml
kubectl apply -f k8s/database/postgres-deployment.yaml
kubectl apply -f k8s/database/postgres-service.yaml
kubectl apply -f k8s/api/api-deployment.yaml
kubectl apply -f k8s/api/api-service.yaml
```

## Components

- **Namespace**: `clean-auth` - Isolates all resources
- **PostgreSQL**:
  - `postgres-pvc.yaml` - Persistent storage
  - `postgres-deployment.yaml` - Database deployment
  - `postgres-service.yaml` - Database service
- **CleanAuth API**:
  - `api-deployment.yaml` - API deployment
  - `api-service.yaml` - API service with NodePort
- **Configuration**:
  - `secrets.yaml` - Sensitive data (database credentials, JWT secret)
```

## Components

- **Infrastructure** (`infrastructure/`):
  - `namespace.yaml` - Kubernetes namespace isolation
  - `secrets.yaml` - Sensitive data (database credentials, JWT secret)
  - `configmap.yaml` - Application configuration
- **Database** (`database/`):
  - `postgres-pvc.yaml` - Persistent storage for PostgreSQL
  - `postgres-deployment.yaml` - PostgreSQL deployment
  - `postgres-service.yaml` - Database service (ClusterIP)
- **API** (`api/`):
  - `api-deployment.yaml` - CleanAuth API deployment
  - `api-service.yaml` - API service with NodePort access
- **Scripts** (`scripts/`):
  - `deploy.sh` - Automated deployment script
  - `cleanup.sh` - Environment cleanup script

## Accessing Services

### NodePort Access (Default)
- API: http://localhost:30001

### Port Forwarding (Alternative)
```bash
# API
kubectl port-forward service/clean-auth-api-service 5001:5001 -n clean-auth

# PostgreSQL (for direct database access)
kubectl port-forward service/postgres-service 5432:5432 -n clean-auth
```

## Useful Commands

```bash
# View all resources
kubectl get all -n clean-auth

# View pods
kubectl get pods -n clean-auth

# View services
kubectl get services -n clean-auth

# View logs
kubectl logs -f deployment/clean-auth-api -n clean-auth
kubectl logs -f deployment/postgres -n clean-auth

# Execute commands in pods
kubectl exec -it deployment/clean-auth-api -n clean-auth -- /bin/bash
kubectl exec -it deployment/postgres -n clean-auth -- psql -U postgres -d CleanAuth

# Describe resources for troubleshooting
kubectl describe pod <pod-name> -n clean-auth
kubectl describe service <service-name> -n clean-auth
```

## Configuration

### Environment Variables
The application uses the following environment variables (stored in secrets):
- `POSTGRES_USER`: postgres
- `POSTGRES_PASSWORD`: strong_password
- `POSTGRES_DB`: CleanAuth
- `JWT_SECRET`: a-very-super-secret-key-that-is-long-enough

### Database Connection
The API connects to PostgreSQL using the service name `postgres-service` on port 5432.

## Persistent Storage

PostgreSQL data is stored in a PersistentVolumeClaim (`postgres-pvc`) with 1Gi of storage.

## Health Checks

Both services include health checks:
- **PostgreSQL**: `pg_isready` command
- **API**: HTTP health endpoint at `/health`

## Scaling

To scale the API:
```bash
kubectl scale deployment clean-auth-api --replicas=3 -n clean-auth
```

## Cleanup

To remove all resources:
```bash
./k8s/scripts/cleanup.sh
```

Or manually:
```bash
kubectl delete namespace clean-auth
```

## Troubleshooting

1. **Image pull errors**: Ensure the Docker image is built locally with `imagePullPolicy: Never`
2. **Pod not starting**: Check logs with `kubectl logs <pod-name> -n clean-auth`
3. **Database connection issues**: Verify PostgreSQL is ready and the connection string is correct
4. **NodePort not accessible**: Ensure your Kubernetes cluster supports NodePort services

## Development Workflow

1. Make code changes
2. Rebuild Docker image: `docker build -t clean-auth-api:latest .`
3. Delete and recreate the API pod: `kubectl delete pod -l app=clean-auth-api -n clean-auth`
4. New pod will use the updated image
