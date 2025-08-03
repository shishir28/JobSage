from fastapi import APIRouter, HTTPException
from pydantic import BaseModel
from app.services.agents.job_categorization_agent import categorization_agent
from app.services.agents.job_summarizer_agent import summarization_agent
from app.services.agents.job_recommendation_agent import recommendation_agent
from fastapi.concurrency import run_in_threadpool

router = APIRouter()

@router.get("/")
async def list_jobs():
    """List jobs - placeholder for future implementation"""
    return {"message": "Jobs endpoint - to be implemented", "jobs": []}


@router.post("/")
async def create_job():
    """Create a job - placeholder for future implementation"""
    return {"message": "Create job endpoint - to be implemented"}

class JobRequest(BaseModel):
    jobId: str
    agent: str
    title: str
    description: str

AGENTS = {
    "summarization": {
        "name": "Summarization Agent",
        "description": "Summarizes maintenance job requests."
    },
    "categorization": {
        "name": "Categorization Agent",
        "description": "Categorizes job requests."
    },
    "recommendation": {
        "name": "Recommendation Agent",
        "description": "Provides Contractor recommendations."
    },
    # Add more agents here
}

@router.get("/agents", tags=["agents"])
async def list_agents():
    """List available agents and their capabilities."""
    return AGENTS

@router.post("/process-job", tags=["jobs"])
async def process_job( job: JobRequest):
    print(f"Processing job: {job.title}, Description: {job.description} and agent: {job.agent}")
    request = {"title": job.title, "description": job.description}
    if job.agent == "categorization":
        result = await run_in_threadpool(categorization_agent.invoke, request)
    elif job.agent == "summarization":
        result = await run_in_threadpool(summarization_agent.invoke, request)
    elif job.agent == "recommendation":
        result = await run_in_threadpool(recommendation_agent.invoke, request)  
    else:
        raise HTTPException(status_code=400, detail="Unknown agent")
    return result
    