#!/bin/bash

# CleanAuth Kubernetes Local Environment Cleanup
set -e

echo "🗑️  Cleaning up CleanAuth Kubernetes environment..."

echo "Deleting API resources..."
kubectl delete -f k8s/api/api-service.yaml --ignore-not-found=true
kubectl delete -f k8s/api/api-deployment.yaml --ignore-not-found=true

echo "Deleting PostgreSQL resources..."
kubectl delete -f k8s/database/postgres-service.yaml --ignore-not-found=true
kubectl delete -f k8s/database/postgres-deployment.yaml --ignore-not-found=true
kubectl delete -f k8s/database/postgres-pvc.yaml --ignore-not-found=true

echo "Deleting Secrets..."
kubectl delete -f k8s/infrastructure/secrets.yaml --ignore-not-found=true

echo "Deleting namespace..."
kubectl delete -f k8s/infrastructure/namespace.yaml --ignore-not-found=true

echo "✅ Cleanup complete!"

# Optional: Remove Docker image
read -p "Do you want to remove the Docker image clean-auth-api:latest? (y/n): " -n 1 -r
echo
if [[ $REPLY =~ ^[Yy]$ ]]; then
    docker rmi clean-auth-api:latest 2>/dev/null || echo "Image not found or already removed"
    echo "🗑️  Docker image removed"
fi
