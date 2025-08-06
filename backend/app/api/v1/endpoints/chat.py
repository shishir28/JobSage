from flask import request,jsonify
from app.chat import ChatArgs, Metadata
from app.chat.chat import build_chat
from fastapi import APIRouter
from pydantic import BaseModel
import uuid

router = APIRouter()

class MessageRequest(BaseModel):
    message: str
    conversation_id: str | None = None
    filter: dict | None = None  # Optional filter for retriever
    namespace: str | None = None  # Optional namespace for retriever

@router.post("/messages")
async def create_message(message_request: MessageRequest):
    conversation_id = message_request.conversation_id or str(uuid.uuid4())
    """Endpoint to send a message in a conversation."""
    
    chat_args = ChatArgs(
        conversation_id=conversation_id,
        metadata=Metadata(conversation_id=conversation_id),
        top_k=5,
        filter=message_request.filter,  # Pass optional filter
        namespace=message_request.namespace,  # Pass optional namespace
    )
    chat = build_chat(chat_args)
  
    try:
        # Run the chat with the message and get the response
        response = chat({"question": message_request.message})
        
        return {
            "conversation_id": conversation_id,
            "message": response["answer"],
            "source_documents": response.get("source_documents", []),
            "generated_question": response.get("generated_question", "")
        }
    except Exception as e:
        print(f"Error in chat response: {e}")
        raise