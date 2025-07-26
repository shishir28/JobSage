from pydantic import BaseModel, Field
from typing import Dict, Any, List, Optional
from datetime import datetime


class HealthResponse(BaseModel):
    """Health check response model"""

    status: str = Field(..., description="Overall health status")
    services: Dict[str, str] = Field(..., description="Individual service statuses")
    message: str = Field(..., description="Health check message")
    timestamp: datetime = Field(default_factory=datetime.now)


class ModelInfo(BaseModel):
    """Model information"""

    name: str = Field(..., description="Model name")
    size: Optional[str] = Field(None, description="Model size")
    digest: Optional[str] = Field(None, description="Model digest")
    modified_at: Optional[datetime] = Field(None, description="Last modified timestamp")


class JobDescriptionRequest(BaseModel):
    """Job description parsing request"""

    job_description: str = Field(..., description="Raw job description text")
    extract_skills: bool = Field(True, description="Extract skills from description")
    extract_requirements: bool = Field(True, description="Extract requirements")
    extract_benefits: bool = Field(True, description="Extract benefits")
    model: Optional[str] = Field(None, description="Specific model to use")


class JobDescriptionResponse(BaseModel):
    """Job description parsing response"""

    job_title: Optional[str] = Field(None, description="Extracted job title")
    company_name: Optional[str] = Field(None, description="Company name")
    location: Optional[str] = Field(None, description="Job location")
    employment_type: Optional[str] = Field(None, description="Employment type")
    experience_level: Optional[str] = Field(
        None, description="Required experience level"
    )
    skills: List[str] = Field(default_factory=list, description="Required skills")
    requirements: List[str] = Field(
        default_factory=list, description="Job requirements"
    )
    benefits: List[str] = Field(default_factory=list, description="Job benefits")
    salary_range: Optional[str] = Field(None, description="Salary range if mentioned")
    summary: Optional[str] = Field(None, description="Job summary")
    processing_time: float = Field(..., description="Processing time in seconds")


class SkillAnalysisRequest(BaseModel):
    """Skill analysis request"""

    text: str = Field(..., description="Text to analyze for skills")
    include_soft_skills: bool = Field(
        True, description="Include soft skills in analysis"
    )
    include_technical_skills: bool = Field(True, description="Include technical skills")
    model: Optional[str] = Field(None, description="Specific model to use")


class SkillAnalysisResponse(BaseModel):
    """Skill analysis response"""

    technical_skills: List[str] = Field(default_factory=list)
    soft_skills: List[str] = Field(default_factory=list)
    skill_categories: Dict[str, List[str]] = Field(default_factory=dict)
    confidence_scores: Dict[str, float] = Field(default_factory=dict)
    processing_time: float = Field(..., description="Processing time in seconds")


class CompletionRequest(BaseModel):
    """Generic completion request"""

    prompt: str = Field(..., description="Input prompt")
    system_prompt: Optional[str] = Field(None, description="System prompt")
    model: Optional[str] = Field(None, description="Model to use")
    temperature: float = Field(
        0.1, ge=0.0, le=2.0, description="Temperature for generation"
    )
    max_tokens: Optional[int] = Field(None, description="Maximum tokens to generate")


class CompletionResponse(BaseModel):
    """Generic completion response"""

    response: str = Field(..., description="Generated response")
    model_used: str = Field(..., description="Model that was used")
    processing_time: float = Field(..., description="Processing time in seconds")
