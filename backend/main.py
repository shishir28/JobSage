from fastapi import FastAPI, HTTPException
from fastapi.middleware.cors import CORSMiddleware
from contextlib import asynccontextmanager
import structlog
from app.core.config import settings
from app.api.v1.router import api_router
from app.core.ollama_client import ollama_client
from dotenv import load_dotenv

load_dotenv()


# Configure structured logging
structlog.configure(
    processors=[structlog.dev.ConsoleRenderer()],
    wrapper_class=structlog.make_filtering_bound_logger(20),  # INFO level
    logger_factory=structlog.PrintLoggerFactory(),
    cache_logger_on_first_use=False,
)

logger = structlog.get_logger()


@asynccontextmanager
async def lifespan(app: FastAPI):
    """Application lifespan events"""
    # Startup
    logger.info("Starting JobSage AI Service...")

    # Test Ollama connection
    try:
        await ollama_client.test_connection()
        logger.info("Ollama connection successful")
    except Exception as e:
        logger.error(f"Ollama connection failed: {e}")

    yield

    # Shutdown
    logger.info("Shutting down JobSage AI Service...")


app = FastAPI(
    title="JobSage AI Service",
    description="Intelligent job description parsing and analysis using LLMs and LangChain",
    version="1.0.0",
    lifespan=lifespan,
    docs_url="/docs" if settings.ENVIRONMENT == "development" else None,
    redoc_url="/redoc" if settings.ENVIRONMENT == "development" else None,
)

# CORS middleware
app.add_middleware(
    CORSMiddleware,
    allow_origins=settings.allowed_origins_list,
    allow_credentials=True,
    allow_methods=["GET", "POST", "PUT", "DELETE"],
    allow_headers=["*"],
)

# Include API router
app.include_router(api_router, prefix="/api/v1")


@app.get("/")
async def root():
    """Health check endpoint"""
    return {
        "message": "JobSage AI Service is running",
        "version": "1.0.0",
        "status": "healthy",
    }


@app.get("/health")
async def health_check():
    """Detailed health check"""
    health_status = {
        "status": "healthy",
        "services": {"api": "healthy", "ollama": "unknown"},
    }

    # Check Ollama health
    try:
        await ollama_client.test_connection()
        health_status["services"]["ollama"] = "healthy"
    except Exception as e:
        health_status["services"]["ollama"] = "unhealthy"
        health_status["status"] = "degraded"
        logger.warning(f"Ollama health check failed: {e}")

    return health_status


if __name__ == "__main__":
    import uvicorn

    uvicorn.run(
        "main:app",
        host=settings.HOST,
        port=settings.PORT,
        reload=settings.RELOAD if settings.ENVIRONMENT == "development" else False,
    )
