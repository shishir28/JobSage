from typing import List, Optional, Any
from langchain_core.vectorstores import VectorStore
from langchain_core.documents import Document
from pinecone import Pinecone
from app.vector_stores.base_store import BaseVectorStore
from app.core.config import settings
import os
import uuid
class PineconeStore(VectorStore, BaseVectorStore):
    def __init__(
        self,
        index_name: str,
        embedding_function = None,
        text_key: str = "text",
    ):
        """Initialize with Pinecone client."""
        super().__init__()
        self.embedding_function = embedding_function
        self.text_key = text_key
        
        pc = Pinecone(
            api_key=settings.PINECONE_API_KEY,
            environment=settings.PINECONE_ENV_NAME            
        )
        self.index = pc.Index(index_name)

    @classmethod
    def from_texts(
        cls,
        texts: List[str],
        embedding: Any,
        metadatas: Optional[List[dict]] = None,
        index_name: Optional[str] = None,
        **kwargs: Any,
    ) -> "PineconeStore":
        """Create a PineconeStore from raw texts.
        
        Args:
            texts: List of texts to add to the store
            embedding: Embedding function to use
            metadatas: Optional list of metadatas associated with the texts
            index_name: Name of the Pinecone index to use
            **kwargs: Additional arguments to pass to the constructor
        
        Returns:
            PineconeStore instance
        """
        # Create embeddings for all texts
        embeddings = [embedding(text) for text in texts]
        
        # Create metadata if none provided
        if metadatas is None:
            metadatas = [{"text": t} for t in texts]
        else:
            # Ensure each metadata dict has a text field
            for i, metadata in enumerate(metadatas):
                metadata["text"] = texts[i]
                
        # Initialize store
        store = cls(index_name=index_name or settings.PINECONE_INDEX_NAME, embedding_function=embedding)
        
        # Add embeddings to store
        store.add(embeddings, metadatas)
        
        return store

    @property
    def embeddings(self):
        """Get the embedding function."""
        return self.embedding_function
    
    def set_embeddings(self, embedding_function):
        """Set the embedding function."""
        self.embedding_function = embedding_function
       

    def add(self, embeddings, metadata):
    # If a single embedding is passed as a flat list, wrap it in another list
        is_list = isinstance(embeddings, list)
        is_not_empty = bool(embeddings)
        first_is_float = is_list and is_not_empty and isinstance(embeddings[0], float)
        if first_is_float:
            embeddings = [embeddings]
        if not isinstance(embeddings, list):
            embeddings = [embeddings]
        if not isinstance(metadata, list):
            metadata = [metadata]

        items = [(item["id"], embedding, item) for embedding, item in zip(embeddings, metadata)]
        self.index.upsert(items)

    def query(self, query_embedding, top_k=5):
        results = self.index.query(vector=query_embedding, top_k=top_k, include_metadata=True)
        return results

    def delete(self, ids):  
        self.index.delete(ids=ids)

    def add_texts(
        self,
        texts: List[str],
        metadatas: Optional[List[dict]] = None,
        **kwargs: Any,
    ) -> List[str]:
        """Add texts to the vector store.
        
        Args:
            texts: List of texts to add
            metadatas: Optional list of metadatas associated with the texts
            **kwargs: Additional arguments to pass to the add method
            
        Returns:
            List of IDs of the added texts
        """
        if not self.embedding_function:
            raise ValueError(
                "embedding_function must be set before adding texts. "
                "This is typically done by passing it to the constructor."
            )
        
        # Create embeddings for all texts
        embeddings = [self.embedding_function(text) for text in texts]
        
        # Create metadata if none provided
        if metadatas is None:
            metadatas = [{"text": t} for t in texts]
        else:
            # Ensure each metadata dict has a text field
            for i, metadata in enumerate(metadatas):
                metadata["text"] = texts[i]
        
        # Generate IDs if not provided in metadata
        ids = [
            metadata.get("id", str(uuid.uuid4()))
            for metadata in metadatas
        ]
        
        # Add to store
        self.add(embeddings, metadatas)
        
        return ids
    def as_retriever(self, search_kwargs: Optional[dict] = None):
        """Return self as retriever."""
        from langchain_core.vectorstores.base import VectorStoreRetriever
        search_kwargs = search_kwargs or {}
        
        return VectorStoreRetriever(
            vectorstore=self,
            search_kwargs=search_kwargs,
            search_type="similarity"
        )

    def similarity_search(
        self,
        query: str,
        k: int = 4,
        filter: Optional[dict] = None,
        namespace: Optional[str] = None,
        **kwargs: Any,
    ) -> List[Document]:
        """Return docs most similar to query."""
        # Generate the embedding for the query
        query_embedding = self.embedding_function(query)
        
        # Set up search parameters
        search_params = {}
        if filter:
            search_params["filter"] = filter
        if namespace:
            search_params["namespace"] = namespace
            
        # Query Pinecone
        results = self.index.query(
            vector=query_embedding,
            top_k=k,
            include_metadata=True,
            **search_params
        )
        
        # Convert results to Documents
        docs = []
        for match in results.matches:
            if match.metadata is None:
                continue
            metadata = match.metadata.copy()
            text = metadata.pop("text", "")
            # Use the ID from metadata as the document ID
            doc = Document(page_content=text, metadata=metadata)
            doc.id = metadata.get("id")  # Set the document ID from metadata
            docs.append(doc)
            
        return docs