from datetime import datetime, timezone
from app.db.models.base import BaseModel
from . import db, SessionLocal
import uuid

def get_or_create_conversation(conversation_id: str) -> "Conversation":
    """Get an existing conversation or create a new one."""
    session = SessionLocal()
    try:
        conversation = session.query(Conversation).filter_by(Id=conversation_id).first()
        if not conversation:
            conversation = Conversation(
                Id=conversation_id,
                Retriever="pinecone",  # Default values
                Memory="sql",
                LLM="ollama"
            )
            session.add(conversation)
            session.commit()
            session.refresh(conversation)
        return conversation
    except Exception as e:
        session.rollback()
        raise e
    finally:
        session.close()

class Conversation(BaseModel):
    """Conversation model for storing chat conversations in a SQL database."""
    __tablename__ = "Conversations"

    Id = db.Column(db.UUID, primary_key=True, default=uuid.uuid4)
    Retriever = db.Column(db.Text, nullable=False)
    Memory = db.Column(db.Text, nullable=False)
    LLM = db.Column(db.Text, nullable=False)
    CreatedAt = db.Column(db.DateTime(timezone=True), default=lambda: datetime.now(timezone.utc))
    UpdatedAt = db.Column(db.DateTime(timezone=True), default=lambda: datetime.now(timezone.utc), onupdate=lambda: datetime.now(timezone.utc))
    messages = db.relationship("Message", back_populates="conversation", order_by="Message.CreatedAt.asc()", cascade="all, delete-orphan")

    def as_dict(self):
        """Convert the conversation to a dictionary format."""
        return {
            "id": self.Id,
            "created_at": self.CreatedAt.isoformat(),
            "updated_at": self.UpdatedAt.isoformat(),
            "messages": [message.as_dict() for message in self.messages]
        }