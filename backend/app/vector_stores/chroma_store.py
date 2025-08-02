from chromadb import Client
from chromadb.config import Settings  # Import the Settings class
from app.vector_stores.base_store import BaseVectorStore
from app.core.config import settings

class ChromaStore(BaseVectorStore):
    def __init__(self, collection_name):
        chromadb_settings = Settings(persist_directory=settings.CHROMA_PERSIST_DIRECTORY,
                                     chroma_api_impl=settings.CHROMA_API_IMPL,
                                     chroma_server_host=settings.CHROMA_HOST,
                                     chroma_server_http_port=settings.CHROMA_PORT,
                                     is_persistent=True)
        self.client = Client(settings=chromadb_settings)
        self.collection = self.client.get_or_create_collection(collection_name)
       

    def add(self, embeddings, metadata):
        ids = [item["id"] for item in metadata]
        self.collection.add(ids=ids, embeddings=embeddings, metadatas=metadata)

    def query(self, query_embedding, top_k=5):
        results = self.collection.query(query_embeddings=[query_embedding], n_results=top_k)
        return results

    def delete(self, ids):
        self.collection.delete(ids=ids)