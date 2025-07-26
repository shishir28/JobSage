import httpx
import structlog
from typing import Dict, Any, List, Optional
from app.core.config import settings

logger = structlog.get_logger()


class OllamaClient:
    """Client for interacting with Ollama LLM service"""

    def __init__(self):
        self.base_url = settings.OLLAMA_BASE_URL
        self.model = settings.OLLAMA_MODEL
        self.timeout = settings.OLLAMA_TIMEOUT

    async def test_connection(self) -> bool:
        """Test connection to Ollama service"""
        try:
            async with httpx.AsyncClient(timeout=5.0) as client:
                response = await client.get(f"{self.base_url}/api/tags")
                return response.status_code == 200
        except Exception as e:
            logger.error(f"Ollama connection test failed: {e}")
            raise

    async def list_models(self) -> List[Dict[str, Any]]:
        """List available models in Ollama"""
        try:
            async with httpx.AsyncClient(timeout=self.timeout) as client:
                response = await client.get(f"{self.base_url}/api/tags")
                response.raise_for_status()
                return response.json().get("models", [])
        except Exception as e:
            logger.error(f"Failed to list Ollama models: {e}")
            raise

    async def generate_completion(
        self,
        prompt: str,
        model: Optional[str] = None,
        system_prompt: Optional[str] = None,
        temperature: float = 0.1,
        max_tokens: Optional[int] = None,
        stream: bool = False,
    ) -> Dict[str, Any]:
        """Generate completion using Ollama"""

        model_to_use = model or self.model

        payload = {
            "model": model_to_use,
            "prompt": prompt,
            "stream": stream,
            "options": {
                "temperature": temperature,
            },
        }

        if system_prompt:
            payload["system"] = system_prompt

        if max_tokens:
            payload["options"]["num_predict"] = max_tokens

        try:
            async with httpx.AsyncClient(timeout=self.timeout) as client:
                response = await client.post(
                    f"{self.base_url}/api/generate", json=payload
                )
                response.raise_for_status()
                return response.json()
        except Exception as e:
            logger.error(f"Ollama completion failed: {e}")
            raise

    async def generate_chat_completion(
        self,
        messages: List[Dict[str, str]],
        model: Optional[str] = None,
        temperature: float = 0.1,
        max_tokens: Optional[int] = None,
        stream: bool = False,
    ) -> Dict[str, Any]:
        """Generate chat completion using Ollama"""

        model_to_use = model or self.model

        payload = {
            "model": model_to_use,
            "messages": messages,
            "stream": stream,
            "options": {
                "temperature": temperature,
            },
        }

        if max_tokens:
            payload["options"]["num_predict"] = max_tokens

        try:
            async with httpx.AsyncClient(timeout=self.timeout) as client:
                response = await client.post(f"{self.base_url}/api/chat", json=payload)
                response.raise_for_status()
                return response.json()
        except Exception as e:
            logger.error(f"Ollama chat completion failed: {e}")
            raise

    async def pull_model(self, model_name: str) -> bool:
        """Pull/download a model to Ollama"""
        payload = {"name": model_name}

        try:
            async with httpx.AsyncClient(
                timeout=300.0
            ) as client:  # Longer timeout for model download
                response = await client.post(f"{self.base_url}/api/pull", json=payload)
                response.raise_for_status()
                return True
        except Exception as e:
            logger.error(f"Failed to pull model {model_name}: {e}")
            raise


# Global Ollama client instance
ollama_client = OllamaClient()
