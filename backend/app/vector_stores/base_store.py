from abc import ABC, abstractmethod

class BaseVectorStore(ABC):
    @abstractmethod
    def add(self, embeddings, metadata):
        """Add embeddings to the vector store."""
        pass

    @abstractmethod
    def query(self, query_embedding, top_k=5):
        """Search for similar embeddings."""
        pass

    @abstractmethod
    def delete(self, ids):
        """Delete embeddings by IDs."""
        pass