from fastapi import APIRouter
from pydantic import BaseModel
from app.services.agents.job_categorization_agent import categorization_agent
from app.services.agents.job_summarizer_agent import summarization_agent
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
    title: str
    description: str


@router.post("/process-job")
async def process_job(job: JobRequest):
    request = {"title": job.title, "description": job.description}
    # result = categorization_agent.invoke(request)
    # result = summarization_agent.invoke(request)
    result = await run_in_threadpool(summarization_agent.invoke, request)
    return result
    