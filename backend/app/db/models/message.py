from app.db.models.base import BaseModel
from datetime import datetime, timezone
from langchain_core.messages import AIMessage, HumanMessage, SystemMessage
from . import db, SessionLocal
import uuid

# Import Conversation model to ensure it's loaded before Message model
from app.db.models.conversation import Conversation

def create_message(**kwargs):
    """Create a new message in the database."""
    session = SessionLocal()
    try:
        message = Message(**kwargs)
        session.add(message)
        session.commit()
        session.refresh(message)
        return message
    except Exception as e:
        session.rollback()
        raise e
    finally:
        session.close()

class Message(BaseModel):
    """Message model for storing chat messages in a SQL database."""
    __tablename__ = "Messages"
    
    Id = db.Column(db.UUID, primary_key=True, default=uuid.uuid4)
    CreatedAt = db.Column(db.DateTime(timezone=True), default=lambda: datetime.now(timezone.utc))
    Role = db.Column(db.Text, nullable=False)
    Content = db.Column(db.Text, nullable=False)
    ConversationId = db.Column(db.UUID, db.ForeignKey('Conversations.Id', ondelete='CASCADE'), nullable=False)
    conversation = db.relationship("Conversation", back_populates="messages")
    
    def as_dict(self):
        """Convert the SQL message to a dictionary format."""
        return {
            "Id": self.Id,
            "CreatedAt": self.CreatedAt.isoformat(),
            "Role": self.Role,
            "Content": self.Content,
            "ConversationId": self.ConversationId
        }

    def as_lc_message(self) -> HumanMessage | AIMessage | SystemMessage:
        """Convert the SQL message to a LangChain message."""
        if self.Role == "human":
            return HumanMessage(content=self.Content)
        elif self.Role == "ai":
            return AIMessage(content=self.Content)
        elif self.Role == "system":
            return SystemMessage(content=self.Content)
        else:
            raise ValueError(f"Unknown role: {self.Role}")
        
    