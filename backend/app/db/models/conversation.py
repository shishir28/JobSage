from pydantic import BaseModel
from datetime import datetime, timezone

from langchain_core.messages import AIMessage, HumanMessage, SystemMessage

import db
import uuid

class Conversation(BaseModel):
    """Conversation model for storing chat conversations in a SQL database."""
    Id: str = db.Column(db.String, primary_key=True, default=lambda: str(uuid.uuid4()))
    CreatedAt: datetime = db.Column(db.DateTime, default=lambda: datetime.now(timezone.utc))
    UpdatedAt: datetime = db.Column(db.DateTime, default=lambda: datetime.now(timezone.utc)) 
    Retriever: str = db.Column(db.String, nullable=False)
    Memory: str = db.Column(db.Text, nullable=False)
    LLM: str = db.Column(db.Text, nullable=False)
    messages = db.relationship("Message", back_populates="conversation", order_by="Message.CreatedAt.asc()")

    def as_dict(self):
        """Convert the SQL message to a dictionary format."""
        return {
            "Id": self.Id,
            "messages": [self.message.as_dict() for message in self.messages]
        }    