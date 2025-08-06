from fastapi import APIRouter
from app.api.v1.endpoints import jobs, health, chat

api_router = APIRouter()

# Include endpoint routers
api_router.include_router(health.router, prefix="/health", tags=["health"])
api_router.include_router(jobs.router, prefix="/jobs", tags=["jobs"])
api_router.include_router(chat.router, prefix="/chat", tags=["chat"])
