from pydantic import BaseModel
from langchain.memory import ChatConversationBufferMemory
from langchain.memory.chat_message_histories import BaseChatMessageHistory
from app.models import Message  # Adjust the import path as needed
import db
class SQLChatMessageHistory(BaseChatMessageHistory):
    """SQL-based chat message history.

    This class extends BaseChatMessageHistory to provide a chat message history that is stored in a SQL database.
    """

    def __init__(self, conversation_id: str**kwargs):
        super().__init__(**kwargs)
        self.conversation_id = conversation_id
        self._messages = []  # Initialize an empty list to hold messages
        # Additional initialization for SQL-specific behavior can be added here.

    @property
    def messages(self):
        """Retrieve messages from the SQL database."""
        try:
            messages = (
                db.session.query(Message)
                .filter_by(conversation_id=self.conversation_id)
                .order_by(Message.created_at.desc())
                .all()
            )
            result = [message.as_lc_message() for message in messages]
            return result if result is not None else []
        except Exception as e:
            print(f"Error retrieving messages: {e}")
            return []
        
    def add_message(self, message: BaseModel):
        """Add a message to the SQL database."""
        try:
            sql_message = Message(
                Role=message.role,
                Content=message.content,
                ConversationId=self.conversation_id
            )
            db.session.add(sql_message)
            db.session.commit()
            self._messages.append(message)  # Update the in-memory list
        except Exception as e:
            print(f"Error adding message: {e}")
            db.session.rollback()

    def add_message(self,message):
        return self.add_message_to_conversation(
            self.conversation_id, 
            message.role, message.content)
 
    def add_user_message(self, content: str) -> Message:
        """Add a user message to the SQL database."""
        return self.add_message_to_conversation(
            self.conversation_id, "user", content)     
    
    def add_ai_message(self, content: str) -> Message:
        """Add an AI message to the SQL database."""
        return self.add_message_to_conversation(
            self.conversation_id, "assistant", content)

    def add_message_to_conversation(self, conversation_id: str, role: str, content: str) -> Message:
        """Add a user message to the SQL database."""
        return Message.create(
            Role=role,
            Content=content,
            ConversationId=conversation_id)
    
    def clear(self):
        """Clear the message history."""
        try:
            db.session.query(Message).filter_by(conversation_id=self.conversation_id).delete()
            db.session.commit()
            self._messages = []  # Clear the in-memory list
        except Exception as e:
            print(f"Error clearing messages: {e}")
            db.session.rollback()
            
def build_sql_memory(conversation_id: str, **kwargs) -> ChatConversationBufferMemory:
    """Build a SQL-based chat memory."""
    return ChatConversationBufferMemory(
        chat_memory=SQLChatMessageHistory(conversation_id=conversation_id, **kwargs),
        return_messages=True,
        memory_key="chat_history",
        input_key="answer"
    )