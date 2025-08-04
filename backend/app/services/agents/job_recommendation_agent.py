from app.vector_stores.pinecone_store import PineconeStore
from app.vector_stores.utils import generate_embedding
from app.core.config import settings  # Adjust the import path if your settings module is elsewhere
from langchain_core.runnables import RunnableMap, RunnableLambda
from app.db.models.contractor import Contractor
from app.services.agents.tools import get_contractor_details_from_db

def analyze_maintenance_job(job_title: str, job_description: str):
    """
    Analyze a maintenance job by categorizing it and providing recommendations.
    """
    combined_text = f"{job_title} {job_description}"
    embedding = generate_embedding(combined_text)

    # Query Pinecone
    pinecone_store = PineconeStore(index_name=settings.PINECONE_INDEX_NAME)    
    top_k = 5  # Set the number of top results to retrieve
    results = pinecone_store.query(embedding, top_k=top_k)

    # Extract required fields from metadata
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

def enrich_with_postgres(recommendations):
    # For each contractorid, query Postgres and add details
    enriched = []
    for rec in recommendations:
        contractorid = rec["contractorid"]
        # Query Postgres for details (use your ORM or db access layer)
        details = get_contractor_details_from_db(contractorid)
        rec.update(details)
        enriched.append(rec)
    return enriched

    
recommendation_agent = (
    RunnableMap({
        "job_title": lambda x: x["title"],
        "job_description": lambda x: x["description"],
    })
    | (lambda x: analyze_maintenance_job(x["job_title"], x["job_description"]))
    | RunnableLambda(enrich_with_postgres)
)
