# JobSage

An intelligent job description parsing and analysis platform that combines Python-based AI with ASP.NET Core to provide agentic capabilities for job seekers and recruiters.

## 🚀 Overview

JobSage is a comprehensive platform that leverages Large Language Models (LLMs) and LangChain to parse, analyze, and extract meaningful insights from job descriptions. The system consists of a Python-based AI backend for natural language processing and an ASP.NET Core web application that provides a user-friendly interface and API endpoints.

## 🏗️ Architecture

```
┌─────────────────────┐    HTTP/API    ┌─────────────────────┐
│                     │    Calls       │                     │
│   ASP.NET Core      │◄──────────────►│   Python AI         │
│   Web Application   │                │   Service           │
│                     │                │   (LangChain + LLM) │
└─────────────────────┘                └─────────────────────┘
         │                                        │
         ▼                                        ▼
┌─────────────────────┐                ┌─────────────────────┐
│                     │                │                     │
│   SQL Server /      │                │   Vector Database   │
│   PostgreSQL        │                │   (Pinecone/Chroma) │
│   Database          │                │                     │
└─────────────────────┘                └─────────────────────┘
```

## ✨ Features

### Core Capabilities
- **Job Description Parsing**: Extract structured data from unstructured job postings
- **Skill Extraction**: Identify required and preferred skills, technologies, and qualifications
- **Salary Analysis**: Parse and normalize salary information
- **Company Intelligence**: Extract company information and culture insights
- **Requirement Matching**: Match candidate profiles against job requirements
- **Trend Analysis**: Identify market trends and in-demand skills

### Agentic Features
- **Smart Recommendations**: AI-powered job recommendations based on candidate profiles
- **Application Optimization**: Suggest resume and cover letter improvements
- **Interview Preparation**: Generate relevant interview questions and preparation materials
- **Career Path Guidance**: Provide career progression insights and skill gap analysis

## 🛠️ Technology Stack

### Backend (Python AI Service)
- **Python 3.11+**
- **FastAPI**: Modern web framework for APIs
- **Pydantic**: Data validation and settings management
- **LangChain**: Framework for developing LLM applications
- **Ollama**: Local LLM runtime (llama2, mistral, etc.)
- **Pipenv**: Python dependency management
- **SQLAlchemy**: Database ORM
- **Redis**: Caching and session management
- **Pandas**: Data manipulation and analysis
- **Structlog**: Structured logging
- **Pytest**: Testing framework

### Frontend (ASP.NET Core Application)
- **ASP.NET Core 8.0+**
- **C# 12**
- **Entity Framework Core**: Database access
- **SignalR**: Real-time communication
- **AutoMapper**: Object mapping
- **Serilog**: Structured logging
- **xUnit**: Unit testing framework

### Infrastructure & Deployment
- **Docker**: Containerization
- **Docker Compose**: Multi-container orchestration
- **Azure/AWS**: Cloud deployment
- **Redis**: Caching and session management
- **NGINX**: Reverse proxy and load balancing

## 📁 Project Structure

```
JobSage/
├── backend/                    # Python AI Service
│   ├── app/
│   │   ├── agents/            # LangChain agents
│   │   ├── chains/            # LangChain chains
│   │   ├── models/            # Data models
│   │   ├── parsers/           # Job description parsers
│   │   ├── services/          # Business logic
│   │   └── utils/             # Utility functions
│   ├── tests/                 # Python tests
│   ├── requirements.txt       # Python dependencies
│   ├── Dockerfile            # Python service Docker config
│   └── main.py               # FastAPI entry point
│
├── frontend/                   # ASP.NET Core Application (Clean Architecture + DDD)
│   ├── src/                  # Source code
│   │   ├── JobSage.Domain/   # Domain Layer (Business Logic)
│   │   │   ├── Entities/     # Domain entities
│   │   │   ├── ValueObjects/ # Value objects
│   │   │   ├── Aggregates/   # Aggregate roots
│   │   │   ├── DomainEvents/ # Domain events
│   │   │   ├── Repositories/ # Repository interfaces
│   │   │   ├── Services/     # Domain services
│   │   │   └── Exceptions/   # Domain exceptions
│   │   ├── JobSage.Application/ # Application Layer (Use Cases)
│   │   │   ├── UseCases/     # Business use cases
│   │   │   │   ├── Jobs/     # Job-related use cases
│   │   │   │   ├── Candidates/ # Candidate use cases
│   │   │   │   └── Analysis/ # Analysis use cases
│   │   │   ├── Commands/     # CQRS Commands
│   │   │   ├── Queries/      # CQRS Queries
│   │   │   ├── DTOs/         # Data Transfer Objects
│   │   │   ├── Interfaces/   # Application interfaces
│   │   │   ├── Services/     # Application services
│   │   │   ├── Behaviors/    # MediatR behaviors
│   │   │   └── Validators/   # FluentValidation validators
│   │   ├── JobSage.Infrastructure/ # Infrastructure Layer
│   │   │   ├── Persistence/  # Database context & repositories
│   │   │   │   ├── Configurations/ # EF configurations
│   │   │   │   └── Repositories/ # Repository implementations
│   │   │   ├── ExternalServices/ # External service integrations
│   │   │   │   └── PythonAI/ # Python AI service client
│   │   │   ├── Caching/      # Redis caching
│   │   │   ├── Logging/      # Serilog configurations
│   │   │   ├── Authentication/ # Auth implementations
│   │   │   └── Messaging/    # Event handling & messaging
│   │   ├── JobSage.API/      # API Presentation Layer
│   │   │   ├── Controllers/  # API controllers
│   │   │   ├── Middleware/   # Custom middleware
│   │   │   ├── Filters/      # Action filters
│   │   │   ├── Extensions/   # Service extensions
│   │   │   └── Configuration/ # API configuration
│   │   └── JobSage.Web/      # Web Presentation Layer
│   │       ├── Controllers/  # MVC controllers
│   │       ├── Views/        # Razor views
│   │       ├── Models/       # View models
│   │       └── wwwroot/      # Static files
│   ├── tests/                # Test projects
│   │   ├── JobSage.Domain.Tests/ # Domain tests
│   │   ├── JobSage.Application.Tests/ # Application tests
│   │   ├── JobSage.Infrastructure.Tests/ # Infrastructure tests
│   │   ├── JobSage.API.Tests/ # API tests
│   │   └── JobSage.Web.Tests/ # Web tests
│   └── JobSage.sln           # Solution file
│
├── docker-compose.yml         # Multi-container setup
├── .env.example              # Environment variables template
├── .gitignore               # Git ignore rules
└── README.md                # Project documentation
```

## 🚀 Getting Started

### Prerequisites
- **Python 3.11+**
- **.NET 8.0 SDK**
- **Docker & Docker Compose**
- **Ollama** (for local LLM)
- **PostgreSQL** (or use Docker)
- **Redis** (or use Docker)

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/yourusername/JobSage.git
   cd JobSage
   ```

2. **Set up environment variables**
   ```bash
   cp .env.example .env
   # Edit .env with your configuration
   ```

3. **Start with Docker Compose**
   ```bash
   docker-compose up -d
   ```

4. **Or run services separately:**

   **First, install and start Ollama:**
   ```bash
   # Install Ollama (macOS/Linux)
   curl -fsSL https://ollama.com/install.sh | sh
   
   # Start Ollama service
   ollama serve
   
   # Pull a model (in another terminal)
   ollama pull llama2
   ```

   **Python AI Service:**
   ```bash
   cd backend
   pipenv install
   pipenv run start
   ```

   **ASP.NET Core Application:**
   ```bash
   cd frontend/src
   dotnet restore
   dotnet run --project JobSage.API
   ```

### Configuration

Create a `.env` file with the following variables:

```env
# LLM Configuration
OPENAI_API_KEY=your_openai_api_key
ANTHROPIC_API_KEY=your_anthropic_api_key

# Database Configuration
DATABASE_URL=postgresql://user:password@localhost:5432/jobsage
SQL_SERVER_CONNECTION=Server=localhost;Database=JobSage;Trusted_Connection=true;

# Vector Database
PINECONE_API_KEY=your_pinecone_api_key
PINECONE_ENVIRONMENT=your_pinecone_environment

# Redis Configuration
REDIS_URL=redis://localhost:6379

# API Configuration
PYTHON_API_BASE_URL=http://localhost:8000
ASPNET_API_BASE_URL=http://localhost:5000
```

## 🔧 API Endpoints

### Python AI Service (Port 8000)

```
POST   /api/parse/job-description     # Parse job description
POST   /api/analyze/skills            # Extract skills from text
POST   /api/match/candidate           # Match candidate to job
POST   /api/generate/recommendations  # Generate job recommendations
GET    /api/trends/skills             # Get skill trends
POST   /api/agents/career-advisor     # Career advice agent
```

### ASP.NET Core API (Port 5000)

```
GET    /api/jobs                      # Get job listings
POST   /api/jobs                      # Create job listing
GET    /api/candidates                # Get candidate profiles
POST   /api/candidates                # Create candidate profile
POST   /api/analysis/job-description  # Analyze job description
GET    /api/reports/market-trends     # Market analysis reports
```

## 🧪 Testing

### Python Tests
```bash
cd backend
pytest tests/ -v
```

### .NET Tests
```bash
cd frontend
dotnet test
```

## 📊 Monitoring & Logging

- **Application Insights**: Performance monitoring
- **Serilog**: Structured logging
- **Health Checks**: Service health monitoring
- **Prometheus**: Metrics collection
- **Grafana**: Dashboards and visualization

## 🚀 Deployment

### Using Docker
```bash
docker-compose -f docker-compose.prod.yml up -d
```

### Azure Deployment
```bash
# Deploy to Azure Container Instances or App Service
az deployment group create --resource-group jobsage-rg --template-file azure-deploy.json
```

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add some amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## 📝 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 📞 Support

- **Issues**: [GitHub Issues](https://github.com/yourusername/JobSage/issues)
- **Documentation**: [Wiki](https://github.com/yourusername/JobSage/wiki)
- **Email**: support@jobsage.com

## 🔮 Roadmap

- [ ] Multi-language support for job descriptions
- [ ] Advanced ML models for salary prediction
- [ ] Integration with major job boards (LinkedIn, Indeed, etc.)
- [ ] Mobile application development
- [ ] Advanced analytics dashboard
- [ ] AI-powered interview scheduling
- [ ] Blockchain-based credential verification

---

**Built with ❤️ by the JobSage Team**
