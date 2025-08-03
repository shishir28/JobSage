from pydantic import BaseModel
from datetime import datetime, timezone

from langchain_core.messages import AIMessage, HumanMessage, SystemMessage

import db
import uuid

class Message(BaseModel):
    """Message model for storing chat messages in a SQL database."""
    Id: str = db.Column(db.String, primary_key=True, default=lambda: str(uuid.uuid4()))
    CreatedAt: datetime = db.Column(db.DateTime, default=lambda: datetime.now(timezone.utc))
    Role: str = db.Column(db.String, nullable=False)
    Content: str = db.Column(db.Text, nullable=False)
    ConversationId: str = db.Column(db.String, db.ForeignKey('Conversation.Id'), nullable=False)
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
        
    