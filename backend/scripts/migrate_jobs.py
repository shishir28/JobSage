
import sys
import os
from decimal import Decimal

# Add the project root to sys.path
sys.path.append(os.path.abspath(os.path.join(os.path.dirname(__file__), "../")))

from  app.db.postgres  import fetch_jobs_from_postgres
from app.vector_stores.utils import generate_embedding
# from app.vector_stores.chroma_store import ChromaStore
from app.vector_stores.pinecone_store import PineconeStore

def main():
    # Step 1: Fetch jobs from PostgreSQL
    jobs = fetch_jobs_from_postgres()
    pinecone_store = PineconeStore(index_name="jobsage")
    for job in jobs:
        combined_text = f"{job['Title']} {job['Description']}"
        embedding = generate_embedding(combined_text)
        metadata = [{
            "id": job['Id'],
            "estimatedcost": float(job['Cost_EstimatedCost']) if isinstance(job['Cost_EstimatedCost'], Decimal) else (job['Cost_EstimatedCost'] or 0.0),
            "actualcost": float(job['Cost_ActualCost']) if isinstance(job['Cost_ActualCost'], Decimal) else (job['Cost_ActualCost'] or 0.0),
            "contractorid": job['ContractorId']
        }]
        pinecone_store.add(embedding, metadata=metadata)

    print("Data migration completed successfully!")

if __name__ == "__main__":
    main()