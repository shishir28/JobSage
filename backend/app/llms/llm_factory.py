from app.core.config import Settings
from langchain_ollama import OllamaLLM

settings = Settings()

llm = OllamaLLM(
    base_url= settings.OLLAMA_BASE_URL,
    model=settings.OLLAMA_MODEL
)