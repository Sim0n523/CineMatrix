# CineMatrix

A cloud-native movie tracking platform with complete DevOps pipeline - from containerization to Kubernetes orchestration with automated CI/CD.

## Description

CineMatrix is a full-stack web application that allows users to discover, track, and rate movies using The Movie Database (TMDb) API. The project demonstrates modern software development practices including containerization with Docker, orchestration with Kubernetes, and automated deployment through GitHub Actions CI/CD pipeline.

**Key Technical Highlights:**
- Multi-stage Docker builds optimized to 215MB (69% size reduction)
- Kubernetes deployment with StatefulSet for persistent data
- Automated CI/CD pipeline via GitHub Actions
- Production-ready architecture with 2 replicas and load balancing

## Screenshots

### Application Interface

<p align="center">
  <img src="screenshots/CineMatrix Home Page.png" width="400" alt="Home Page"/>
  <img src="screenshots/CineMatrix Recommendations.png" width="400" alt="Recommendations"/>
  <img src="screenshots/CineMatrix Movie Review.png" width="400" alt="Movie Review"/>
</p>

## Features

- **Movie Discovery** - Browse and search 500,000+ movies via TMDb API
- **Personal Tracking** - Watchlist management and viewing history
- **Rating System** - Rate movies on a 1-10 scale with personal reviews
- **Recommendations** - Personalized suggestions based on preferences
- **Cloud-Native** - Containerized deployment with horizontal scaling
- **Automated CI/CD** - GitHub Actions pipeline for continuous deployment

## Tech Stack

**Application:**
- ASP.NET Core 8.0 (C#)
- Entity Framework Core
- SQL Server 2022
- Razor Views + Bootstrap 5
- TMDb API

**DevOps & Infrastructure:**
- Docker (Multi-stage builds)
- Docker Compose
- Kubernetes (Minikube/Docker Desktop)
- GitHub Actions (CI/CD)
- NGINX Ingress Controller
- Docker Hub (Registry)

## Architecture

```

GitHub Repository
↓
GitHub Actions CI/CD Pipeline
├── Build & Test
├── Docker Build (multi-stage)
└── Push to Docker Hub
↓
Kubernetes Cluster
├── Namespace: cinematrix
├── StatefulSet: SQL Server (10Gi persistent storage)
├── Deployment: Web App (2 replicas)
├── Services: ClusterIP + Headless
├── ConfigMaps & Secrets
└── Ingress: NGINX (cinematrix.local)

```

## Installation

### Prerequisites
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/)
- [kubectl](https://kubernetes.io/docs/tasks/tools/)
- [Minikube](https://minikube.sigs.k8s.io/) or Docker Desktop with Kubernetes enabled
- [TMDb API Key](https://www.themoviedb.org/settings/api) (free)

### Setup

```bash
# Clone repository
git clone https://github.com/Sim0n523/CineMatrix.git
cd CineMatrix

# Configure environment variables
cp .env.example .env
# Edit .env and add your TMDb API key
```

## Usage

### Option 1: Docker Compose (Local Development)

```bash
# Start all services (web app + SQL Server)
docker-compose up -d

# View logs
docker-compose logs -f

# Access application
http://localhost:5000

# Stop services
docker-compose down
```

### Option 2: Kubernetes (Production-like)

```bash
# Start Kubernetes cluster
minikube start --memory=4096 --cpus=2

# Deploy application
kubectl apply -f k8s/namespace.yaml
kubectl apply -f k8s/database/
kubectl apply -f k8s/app/
kubectl apply -f k8s/ingress.yaml

# Enable tunnel (for Minikube)
minikube tunnel

# Configure DNS
# Add to /etc/hosts (macOS/Linux) or C:\Windows\System32\drivers\etc\hosts (Windows):
# 127.0.0.1 cinematrix.local   # Docker Desktop
# 192.168.49.2 cinematrix.local # Minikube (use minikube ip to get IP)

# Access application
http://cinematrix.local

# View deployment status
kubectl get all -n cinematrix
```

### Option 3: Local Development (.NET)

```bash
# Restore dependencies
dotnet restore

# Apply database migrations
dotnet ef database update --project CineMatrix.Repository --startup-project CineMatrix

# Run application
cd CineMatrix
dotnet run

# Access at https://localhost:7016
```

## Project Structure

```

CineMatrix/
├── CineMatrix/                  # MVC Controllers & Razor Views
├── CineMatrix.Service/          # Business logic & DTOs
├── CineMatrix.Repository/       # Data access & EF Core
├── CineMatrix.Domain/           # Core entities
├── Dockerfile                   # Multi-stage Docker build
├── docker-compose.yml           # Local orchestration
├── .github/workflows/
│   └── ci-cd.yml               # GitHub Actions pipeline
├── k8s/                        # Kubernetes manifests
│   ├── namespace.yaml          # Namespace: cinematrix
│   ├── app/
│   │   ├── deployment.yaml     # Web app (2 replicas)
│   │   ├── service.yaml        # ClusterIP service
│   │   ├── configmap.yaml      # Configuration
│   │   └── secret.yaml         # Sensitive data
│   ├── database/
│   │   ├── statefulset.yaml    # SQL Server (persistent)
│   │   ├── service.yaml        # Headless service
│   │   ├── configmap.yaml      # DB configuration
│   │   └── secret.yaml         # DB credentials
│   └── ingress.yaml            # NGINX ingress routing
└── screenshots/                # Application screenshots

```

## Key DevOps Features

### Docker Multi-Stage Build
- **Build Stage**: Uses .NET SDK 8.0 (700MB) for compilation
- **Runtime Stage**: Uses .NET Runtime (215MB) for execution
- **Result**: 69% reduction in image size

### Kubernetes Resources

| Resource | Configuration |
|----------|---------------|
| **Web App** | 2 replicas, 256Mi-512Mi RAM, session affinity enabled |
| **Database** | StatefulSet with 10Gi PVC, 2Gi-4Gi RAM |
| **Networking** | ClusterIP services, NGINX Ingress |
| **Configuration** | ConfigMaps for settings, Secrets for credentials |

### CI/CD Pipeline Stages
Push to main → Build & Test → Docker Build → Push to Registry → Complete
