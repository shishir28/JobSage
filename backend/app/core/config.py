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
    
    LANGCHAIN_VERBOSE: str = os.getenv("LANGCHAIN_VERBOSE")
    LANGCHAIN_DEBUG: str = os.getenv("LANGCHAIN_DEBUG")

    POSTGRES_USER: str = os.getenv("POSTGRES_USER")
    POSTGRES_PASSWORD: str = os.getenv("POSTGRES_PASSWORD")
    POSTGRES_DB: str = os.getenv("POSTGRES_DB")
    POSTGRES_HOST: str = os.getenv("POSTGRES_HOST")
    POSTGRES_PORT: int = os.getenv("POSTGRES_PORT")
    
    CHROMA_API_IMPL: str = os.getenv("CHROMA_API_IMPL")
    CHROMA_HOST: str = os.getenv("CHROMA_HOST")
    CHROMA_PORT: int = os.getenv("CHROMA_PORT")
    CHROMA_COLLECTION_NAME: str = os.getenv("CHROMA_COLLECTION_NAME")
    CHROMA_PERSIST: bool = os.getenv("CHROMA_PERSIST", "True").lower() in ("true", "1", "yes")
    CHROMA_PERSIST_DIRECTORY: str = os.getenv("CHROMA_PERSIST_DIRECTORY")

    class Config:
        env_file = ".env"
        case_sensitive = True


settings = Settings()
