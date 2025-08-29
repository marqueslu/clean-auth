#!/bin/bash

# CleanAuth Kubernetes Local Environment Setup
set -e

echo "🚀 Setting up CleanAuth Kubernetes local environment..."

# Check if kubectl is available
if ! command -v kubectl &> /dev/null; then
    echo "❌ kubectl is not installed. Please install kubectl first."
    exit 1
fi

# Check if Docker is running
if ! docker info &> /dev/null; then
    echo "❌ Docker is not running. Please start Docker first."
    exit 1
fi

echo "📦 Building Docker image..."
docker build -t clean-auth-api:latest .

# Check if we're using kind and load the image
if kubectl config current-context | grep -q "kind"; then
    echo "🔄 Loading Docker image into kind cluster..."
    kind load docker-image clean-auth-api:latest --name $(kubectl config current-context | sed 's/kind-//')
fi

echo "🏗️  Applying Kubernetes manifests..."

# Apply in order
kubectl apply -f k8s/infrastructure/namespace.yaml
kubectl apply -f k8s/infrastructure/secrets.yaml
kubectl apply -f k8s/database/postgres-pvc.yaml
kubectl apply -f k8s/database/postgres-deployment.yaml
kubectl apply -f k8s/database/postgres-service.yaml
kubectl apply -f k8s/api/api-deployment.yaml
kubectl apply -f k8s/api/api-service.yaml

echo "⏳ Waiting for PostgreSQL to be ready..."
kubectl wait --for=condition=ready pod -l app=postgres -n clean-auth --timeout=120s

echo "⏳ Waiting for API to be ready..."
kubectl wait --for=condition=ready pod -l app=clean-auth-api -n clean-auth --timeout=120s

echo "✅ Deployment complete!"
echo ""
echo "🔗 Access your application:"
echo "   API: http://localhost:30001"
echo ""
echo "📊 Useful commands:"
echo "   View pods: kubectl get pods -n clean-auth"
echo "   View services: kubectl get services -n clean-auth"
echo "   View logs (API): kubectl logs -f deployment/clean-auth-api -n clean-auth"
echo "   View logs (PostgreSQL): kubectl logs -f deployment/postgres -n clean-auth"
echo "   Port forward API: kubectl port-forward service/clean-auth-api-service 5001:5001 -n clean-auth"
echo "   Port forward PostgreSQL: kubectl port-forward service/postgres-service 5432:5432 -n clean-auth"
echo ""
echo "🗑️  To cleanup: ./k8s/scripts/cleanup.sh"
