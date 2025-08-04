
from app.vector_stores.pinecone_store import PineconeStore
from app.vector_stores.utils import generate_embedding
from app.core.config import settings
from langchain_core.runnables import RunnableMap, RunnableLambda
from app.services.agents.tools import get_contractor_details_from_db

class ContractorRetriever:
    def __init__(self, index_name=None, top_k=5):
        self.index_name = index_name or settings.PINECONE_INDEX_NAME
        self.top_k = top_k
        self.pinecone_store = PineconeStore(index_name=self.index_name)

    def retrieve(self, job_title: str, job_description: str):
        combined_text = f"{job_title} {job_description}"
        embedding = generate_embedding(combined_text)
        results = self.pinecone_store.query(embedding, top_k=self.top_k)
        matches = results.get("matches", [])
        recommendations = []
        for match in matches:
            metadata = match.get("metadata", {})
            recommendations.append({
                "contractorid": metadata.get("contractorid"),
                # "actualcost": metadata.get("actualcost"),
                "estimatedcost": metadata.get("estimatedcost"),
            })
        return recommendations

    def enrich_with_postgres(self, recommendations):
        enriched = []
        for rec in recommendations:
            contractorid = rec["contractorid"]
            details = get_contractor_details_from_db(contractorid)
            rec.update(details)
            enriched.append(rec)
        return enriched

# Example: you can pass custom config if needed
retriever = ContractorRetriever(top_k=5)

recommendation_agent = (
    RunnableMap({
        "job_title": lambda x: x["title"],
        "job_description": lambda x: x["description"],
    })
    | (lambda x: retriever.retrieve(x["job_title"], x["job_description"]))
    | RunnableLambda(retriever.enrich_with_postgres)
)
