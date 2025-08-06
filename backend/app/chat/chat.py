from app.chat import ChatArgs
from app.vector_stores.pinecone_store import PineconeStore
from app.vector_stores.utils import generate_embedding
from app.chat.memories.sql_memory import build_sql_memory
from langchain.chains.conversational_retrieval.base import ConversationalRetrievalChain
from app.llms.llm_factory import llm
from app.core.config import settings

from app.services.agents.tools import get_contractor_details_from_db

def build_chat(chat_args: ChatArgs):
    """Build a conversational retrieval chain with memory."""
    # Initialize the vector store with embedding function
    pinecone_store = PineconeStore(
        index_name=settings.PINECONE_INDEX_NAME,
        embedding_function=generate_embedding
    )

    # Custom retriever function to enrich results with contractor details
    def get_relevant_docs_with_contractor_details(query):
        # Get docs from vector store
        docs = pinecone_store.similarity_search(
            query,
            k=chat_args.top_k or 5,
            filter=chat_args.filter,
            namespace=chat_args.namespace
        )
        
        # Enrich each document with contractor details
        for doc in docs:
            if 'contractorid' in doc.metadata:
                contractor_details = get_contractor_details_from_db(doc.metadata['contractorid'])
                doc.metadata.update(contractor_details)
                # Add contractor details to the page content for the LLM to use
                doc.page_content = f"""
Job Details:
- Estimated Cost: ${doc.metadata.get('estimatedcost', 'N/A')}
- Actual Cost: ${doc.metadata.get('actualcost', 'N/A')}

Contractor Information:
- Name: {contractor_details.get('name', 'N/A')}
- Location: {contractor_details.get('location', 'N/A')}
- Hourly Rate: ${contractor_details.get('hourlyrate', 'N/A')}
- Rating: {contractor_details.get('rating', 'N/A')}
- Contact Info: {contractor_details.get('contactinfo', 'N/A')}
"""
        return docs

    # Create a custom retriever class that inherits from VectorStoreRetriever
    from langchain_core.vectorstores.base import VectorStoreRetriever
    
    class CustomRetriever(VectorStoreRetriever):
        def get_relevant_documents(self, query: str):
            return get_relevant_docs_with_contractor_details(query)
    
    # Create the retriever using our custom class
    vector_store_retriever = CustomRetriever(
        vectorstore=pinecone_store,
        search_kwargs={
            "k": chat_args.top_k or 5,
            "filter": chat_args.filter,
            "namespace": chat_args.namespace
        },
        search_type="similarity"
    )
    
    # Initialize memory
    memory = build_sql_memory(chat_args)
    
    from langchain_core.prompts import PromptTemplate
    
    # Create the prompt template for handling both cost and contractor information
    qa_prompt = PromptTemplate.from_template(
        '''You are a helpful assistant that provides information about jobs, costs, and contractors.
        Use the following context to answer the user's question.
        
        Chat History: {chat_history}
        User Question: {question}
        Similar Jobs and Contractors: {context}
        
        Instructions:
        1. When discussing costs:
           - Always provide the range of estimated costs (lowest to highest)
           - Calculate and show the average cost
           - Mention actual costs if available
        
        2. When discussing contractors:
           - Include contractor names and locations
           - Mention their ratings and hourly rates
           - Provide their contact information
        
        3. Always try to give a complete picture by combining:
           - Cost information (estimates, ranges, averages)
           - Contractor details (experience, ratings, location)
           - Any relevant job-specific information
        
        Answer: Based on the similar jobs in our database, here's what I found:'''
    )

    # Create and return the conversational chain
    chain = ConversationalRetrievalChain.from_llm(
        llm=llm,
        retriever=vector_store_retriever,
        memory=memory,
        combine_docs_chain_kwargs={"prompt": qa_prompt},
        verbose=True
    )
    
    return chain