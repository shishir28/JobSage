from pydantic_settings import BaseSettings
from typing import List
import os


class Settings(BaseSettings):
    """Application settings"""

    # Environment
    ENVIRONMENT: str = os.getenv("ENVIRONMENT")

    # Server Configuration
    HOST: str = os.getenv("HOST")
    PORT: int = os.getenv("PORT")
    RELOAD: bool = os.getenv("RELOAD")

    # Ollama Configuration
    OLLAMA_BASE_URL: str = os.getenv("OLLAMA_BASE_URL")
    OLLAMA_MODEL: str = os.getenv("OLLAMA_MODEL")
    OLLAMA_TIMEOUT: int = os.getenv("OLLAMA_TIMEOUT")

    # API Configuration
    API_V1_STR: str = "/api/v1"
    PROJECT_NAME: str = "JobSage AI Service"

    # CORS - Parse comma-separated string into list
    ALLOWED_ORIGINS: str = os.getenv("ALLOWED_ORIGINS")

    @property
    def allowed_origins_list(self) -> List[str]:
        """Convert comma-separated origins string to list"""
        return [
            origin.strip()
            for origin in self.ALLOWED_ORIGINS.split(",")
            if origin.strip()
        ]

    # Database Configuration
    DATABASE_URL: str = os.getenv("DATABASE_URL")

    # Redis Configuration (for caching)
    REDIS_URL: str = os.getenv("REDIS_URL")

    # LangChain Configuration
    LANGCHAIN_TRACING_V2: bool = os.getenv("LANGCHAIN_TRACING_V2")
    LANGCHAIN_API_KEY: str = os.getenv("LANGCHAIN_API_KEY")

    # Logging
    LOG_LEVEL: str = os.getenv("LOG_LEVEL", "INFO")

    # OpenAI
    OPENAI_API_KEY: str = os.getenv("OPENAI_API_KEY")

    # Pinecone
    PINECONE_API_KEY: str = os.getenv("PINECONE_API_KEY")
    PINECONE_ENV_NAME: str = os.getenv("PINECONE_ENV_NAME")
    PINECONE_INDEX_NAME: str = os.getenv("PINECONE_INDEX_NAME")

    class Config:
        env_file = ".env"
        case_sensitive = True


settings = Settings()
