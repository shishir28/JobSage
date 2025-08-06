from app.chat import ChatArgs
from app.vector_stores.pinecone_store import PineconeStore
from app.vector_stores.utils import generate_embedding
from app.chat.memories.sql_memory import build_sql_memory
from langchain.chains.conversational_retrieval.base import ConversationalRetrievalChain
from app.llms.llm_factory import llm
from app.core.config import settings

def build_chat(chat_args: ChatArgs):
    """Build a conversational retrieval chain with memory."""
    # Initialize the vector store with embedding function
    pinecone_store = PineconeStore(
        index_name=settings.PINECONE_INDEX_NAME,
        embedding_function=generate_embedding
    )
    
    # Create the retriever with search parameters
    vector_store_retriever = pinecone_store.as_retriever(
        search_kwargs={
            "k": chat_args.top_k or 5,
            "filter": chat_args.filter,
            "namespace": chat_args.namespace
        }
    )
    
    # Initialize memory
    memory = build_sql_memory(chat_args)
    
    # Build and return the chain
    return ConversationalRetrievalChain.from_llm(
        llm=llm,
        retriever=vector_store_retriever,
        memory=memory,
        return_source_documents=True,
        return_generated_question=True,
    )