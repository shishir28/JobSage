from langchain.memory import ConversationBufferMemory
from langchain_core.chat_history import BaseChatMessageHistory
from langchain_core.messages import BaseMessage, HumanMessage, AIMessage
from app.db.models.message import Message, create_message
from app.db.models import SessionLocal
from app.chat import ChatArgs

class SQLChatMessageHistory(BaseChatMessageHistory):
    """Chat message history that stores messages in a PostgreSQL database."""
    
    def __init__(self, conversation_id: str):
        self.conversation_id = conversation_id
        # Ensure conversation exists
        from app.db.models.conversation import get_or_create_conversation
        get_or_create_conversation(conversation_id)

    @property
    def messages(self) -> list[BaseMessage]:
        """Retrieve messages from the SQL database."""
        session = SessionLocal()
        try:
            # Query messages and order by creation time
            messages = (
                session.query(Message)
                .filter_by(ConversationId=self.conversation_id)
                .order_by(Message.CreatedAt.asc())  # Get messages in chronological order
                .all()
            )
            return [message.as_lc_message() for message in messages] if messages else []
        except Exception as e:
            print(f"Error retrieving messages: {e}")
            return []
        finally:
            session.close()

    def add_user_message(self, content: str) -> None:
        """Add a human message to the store."""
        create_message(
            Role="human",
            Content=content,
            ConversationId=self.conversation_id
        )

    def add_ai_message(self, content: str) -> None:
        """Add an AI message to the store."""
        create_message(
            Role="ai",
            Content=content,
            ConversationId=self.conversation_id
        )

    def add_message(self, message: BaseMessage) -> None:
        """Add a message to the store."""
        if isinstance(message, HumanMessage):
            self.add_user_message(message.content)
        elif isinstance(message, AIMessage):
            self.add_ai_message(message.content)
        else:
            raise ValueError(f"Unsupported message type: {type(message)}")

    def clear(self) -> None:
        """Clear message history."""
        session = SessionLocal()
        try:
            session.query(Message).filter_by(ConversationId=self.conversation_id).delete()
            session.commit()
        except Exception as e:
            print(f"Error clearing messages: {e}")
            session.rollback()
            raise
        finally:
            session.close()

def build_sql_memory(chat_args: ChatArgs) -> ConversationBufferMemory:
    """Build a SQL-based chat memory."""
    return ConversationBufferMemory(
        chat_memory=SQLChatMessageHistory(conversation_id=chat_args.conversation_id),
        return_messages=True,
        memory_key="chat_history",
        input_key="question",
        output_key="answer"
    )