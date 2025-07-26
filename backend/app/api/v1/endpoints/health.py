from fastapi import APIRouter, HTTPException
from typing import List, Dict, Any
from app.core.ollama_client import ollama_client
from app.models.base import HealthResponse, ModelInfo

router = APIRouter()


@router.get("/", response_model=HealthResponse)
async def health_check():
    """Detailed health check for the AI service"""
    try:
        # Test Ollama connection
        is_ollama_healthy = await ollama_client.test_connection()

        return HealthResponse(
            status="healthy" if is_ollama_healthy else "degraded",
            services={
                "ollama": "healthy" if is_ollama_healthy else "unhealthy",
                "api": "healthy",
            },
            message="AI service is operational",
        )
    except Exception as e:
        return HealthResponse(
            status="unhealthy",
            services={"ollama": "unhealthy", "api": "healthy"},
            message=f"Health check failed: {str(e)}",
        )
