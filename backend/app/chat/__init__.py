# JobSage AI Backend Application
from pydantic import BaseModel, Extra

class Metadata(BaseModel, extra=Extra.allow):
    conversation_id: str
    

class ChatArgs(BaseModel, extra=Extra.allow):
    """Arguments for the chat endpoint."""
    conversation_id: str
    metadata: Metadata = None
    top_k: int = 5
    filter: dict | None = None  # Optional filter for retriever
    namespace: str | None = None  # Optional namespace for retriever
