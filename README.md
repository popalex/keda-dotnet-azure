# keda-dotnet-azure
Keda Dotnet Azure example

# Run the Producer:

```bash
export AZURE_STORAGE_CONNECTION_STRING="<your-azure-storage-connection-string>"
dotnet run
```


# Run the KEDA Workers:

```bash
export AZURE_STORAGE_CONNECTION_STRING="<your-azure-storage-connection-string>"
dotnet run
```

# Build & Push the docker Image

```bash
docker build -t myregistry.azurecr.io/kedaworker:latest .
docker push myregistry.azurecr.io/kedaworker:latest
```

# Deploy the kubernetes:

```bash
kubectl apply -f secret.yaml
kubectl apply -f deployment.yaml
```

# Configure KEDA to Auto-Scale the Worker

```bash
kubectl apply -f scaledobject.yaml
```

# Test the Autoscaling

1. Send events to the queue:

```bash
export AZURE_STORAGE_CONNECTION_STRING="<your-azure-storage-connection-string>"
dotnet run
```

2. Check pod scaling:

```bash
kubectl get pods -w
```
