from fastapi import APIRouter, HTTPException
import time
import json
from app.core.ollama_client import ollama_client
from app.models.base import (
    JobDescriptionRequest,
    JobDescriptionResponse,
    SkillAnalysisRequest,
    SkillAnalysisResponse,
    CompletionRequest,
    CompletionResponse,
)

router = APIRouter()


@router.post("/parse-job-description", response_model=JobDescriptionResponse)
async def parse_job_description(request: JobDescriptionRequest):
    """Parse a job description and extract structured information"""
    start_time = time.time()

    try:
        # Create system prompt for job description parsing
        system_prompt = """You are an expert job description analyzer. Extract the following information from the job description and return it as a valid JSON object:

{
    "job_title": "extracted job title",
    "company_name": "company name if mentioned",
    "location": "job location if mentioned", 
    "employment_type": "full-time/part-time/contract/etc",
    "experience_level": "entry/mid/senior/etc",
    "skills": ["skill1", "skill2", "..."],
    "requirements": ["requirement1", "requirement2", "..."],
    "benefits": ["benefit1", "benefit2", "..."],
    "salary_range": "salary range if mentioned",
    "summary": "brief job summary"
}

Return only the JSON object, no other text."""

        # Generate completion
        response = await ollama_client.generate_completion(
            prompt=request.job_description,
            system_prompt=system_prompt,
            model=request.model,
            temperature=0.1,
        )

        # Parse the response
        llm_response = response.get("response", "")

        # Try to extract JSON from the response
        try:
            # Find JSON in the response
            start_idx = llm_response.find("{")
            end_idx = llm_response.rfind("}") + 1

            if start_idx != -1 and end_idx != -1:
                json_str = llm_response[start_idx:end_idx]
                parsed_data = json.loads(json_str)
            else:
                # Fallback: create a basic response
                parsed_data = {
                    "job_title": None,
                    "company_name": None,
                    "location": None,
                    "employment_type": None,
                    "experience_level": None,
                    "skills": [],
                    "requirements": [],
                    "benefits": [],
                    "salary_range": None,
                    "summary": (
                        llm_response[:200] + "..."
                        if len(llm_response) > 200
                        else llm_response
                    ),
                }
        except json.JSONDecodeError:
            # Fallback response
            parsed_data = {
                "job_title": None,
                "company_name": None,
                "location": None,
                "employment_type": None,
                "experience_level": None,
                "skills": [],
                "requirements": [],
                "benefits": [],
                "salary_range": None,
                "summary": "Failed to parse job description",
            }

        processing_time = time.time() - start_time

        return JobDescriptionResponse(**parsed_data, processing_time=processing_time)

    except Exception as e:
        raise HTTPException(
            status_code=500, detail=f"Job description parsing failed: {str(e)}"
        )


@router.post("/analyze-skills", response_model=SkillAnalysisResponse)
async def analyze_skills(request: SkillAnalysisRequest):
    """Analyze text to extract and categorize skills"""
    start_time = time.time()

    try:
        system_prompt = """You are an expert skills analyzer. Analyze the given text and extract skills, categorizing them as technical or soft skills. Return the result as a valid JSON object:

{
    "technical_skills": ["Python", "JavaScript", "..."],
    "soft_skills": ["Communication", "Leadership", "..."], 
    "skill_categories": {
        "Programming Languages": ["Python", "Java"],
        "Frameworks": ["React", "Django"],
        "Databases": ["PostgreSQL", "MongoDB"],
        "Cloud": ["AWS", "Azure"],
        "Soft Skills": ["Communication", "Teamwork"]
    },
    "confidence_scores": {
        "Python": 0.95,
        "Communication": 0.8
    }
}

Return only the JSON object, no other text."""

        response = await ollama_client.generate_completion(
            prompt=request.text,
            system_prompt=system_prompt,
            model=request.model,
            temperature=0.1,
        )

        llm_response = response.get("response", "")

        try:
            start_idx = llm_response.find("{")
            end_idx = llm_response.rfind("}") + 1

            if start_idx != -1 and end_idx != -1:
                json_str = llm_response[start_idx:end_idx]
                parsed_data = json.loads(json_str)
            else:
                parsed_data = {
                    "technical_skills": [],
                    "soft_skills": [],
                    "skill_categories": {},
                    "confidence_scores": {},
                }
        except json.JSONDecodeError:
            parsed_data = {
                "technical_skills": [],
                "soft_skills": [],
                "skill_categories": {},
                "confidence_scores": {},
            }

        processing_time = time.time() - start_time

        return SkillAnalysisResponse(**parsed_data, processing_time=processing_time)

    except Exception as e:
        raise HTTPException(status_code=500, detail=f"Skill analysis failed: {str(e)}")


@router.post("/completion", response_model=CompletionResponse)
async def generate_completion(request: CompletionRequest):
    """Generate a generic completion using Ollama"""
    start_time = time.time()

    try:
        response = await ollama_client.generate_completion(
            prompt=request.prompt,
            system_prompt=request.system_prompt,
            model=request.model,
            temperature=request.temperature,
            max_tokens=request.max_tokens,
        )

        processing_time = time.time() - start_time

        return CompletionResponse(
            response=response.get("response", ""),
            model_used=response.get("model", request.model or "default"),
            processing_time=processing_time,
        )

    except Exception as e:
        raise HTTPException(
            status_code=500, detail=f"Completion generation failed: {str(e)}"
        )
