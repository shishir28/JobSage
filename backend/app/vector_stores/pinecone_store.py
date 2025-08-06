from pinecone import Pinecone

from app.vector_stores.base_store import BaseVectorStore
from app.core.config import settings

import os
class PineconeStore(BaseVectorStore):
    def __init__(self, index_name):
        pc = Pinecone(
            api_key=settings.PINECONE_API_KEY,
            environment=settings.PINECONE_ENV_NAME            
        )
        self.index = pc.Index(index_name)
       

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