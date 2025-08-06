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
        
        # Sort docs by similarity score if available
        if docs and hasattr(docs[0], 'score'):
            docs = sorted(docs, key=lambda x: x.score, reverse=True)
        
        
        # Enrich each document with contractor details
        for doc in docs:
            if 'contractorid' in doc.metadata:
                contractor_details = get_contractor_details_from_db(doc.metadata['contractorid'])
                doc.metadata.update(contractor_details)
                # Add contractor details to the page content for the LLM to use
                # Format the content to include the full job description and metadata
                content_parts = []
                
                # Include the original job description first for better semantic matching
                if doc.page_content:
                    content_parts.append("Job Description:")
                    content_parts.append(doc.page_content)
                
                content_parts.append("\nJob Details:")
                # Only use the metadata fields we know exist
                if doc.metadata.get('estimatedcost'):
                    content_parts.append(f"- Estimated Cost: ${doc.metadata['estimatedcost']}")
                if doc.metadata.get('actualcost') and doc.metadata['actualcost'] > 0:
                    content_parts.append(f"- Actual Cost: ${doc.metadata['actualcost']}")
                
                # Add contractor information if available
                if contractor_details:
                    content_parts.append("\nContractor Information:")
                    
                    # Name and Trade
                    if contractor_details.get('name'):
                        content_parts.append(f"- Name: {contractor_details['name']}")
                    if contractor_details.get('trade'):
                        content_parts.append(f"- Trade: {contractor_details['trade']}")
                    
                    # Location and Contact
                    if contractor_details.get('location'):
                        content_parts.append(f"- Location: {contractor_details['location']}")
                    if contractor_details.get('contactinfo'):
                        content_parts.append(f"- Contact: {contractor_details['contactinfo']}")
                    
                    # Professional Details
                    if contractor_details.get('hourlyrate'):
                        content_parts.append(f"- Hourly Rate: ${contractor_details['hourlyrate']}")
                    if contractor_details.get('rating'):
                        content_parts.append(f"- Rating: {contractor_details['rating']} stars")
                    if contractor_details.get('availability'):
                        content_parts.append(f"- Availability: {contractor_details['availability']}")
                    
                    # Additional Qualifications
                    qualifications = []
                    if contractor_details.get('preferred'):
                        qualifications.append("Preferred Contractor")
                    if contractor_details.get('warrantyApproved'):
                        qualifications.append("Warranty Approved")
                    if qualifications:
                        content_parts.append("- Qualifications: " + ", ".join(qualifications))
                
                doc.page_content = "\n".join(content_parts)
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
        '''You are a helpful assistant that provides information about jobs and contractors in Australia.
        Use ONLY the following context to answer the user's question. DO NOT make up or infer any additional information.
        
        Chat History: {chat_history}
        User Question: {question}
        Similar Jobs and Contractors: {context}
        
        Instructions:
        1. When discussing costs:
           - Only mention costs that are explicitly provided in the context
           - If showing a range or average, use only the actual numbers given
           - Do not estimate or infer costs that aren't shown
        
        2. When discussing contractors:
           - Only include information explicitly provided in the context
           - Never make up or infer contact details, addresses, or phone numbers
           - Only mention Australian locations that are specifically given
           - If information is not provided, say "This information is not available"
        
        3. Important rules:
           - Never generate or infer missing information
           - Never create fictional addresses or phone numbers
           - Only use contractor details exactly as provided from the database
           - If asked about information that isn't in the context, say "I don't have that information"
           - All contractors are based in Australia - never suggest US or other locations
        
        Answer: Based strictly on the information provided in our database:'''
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