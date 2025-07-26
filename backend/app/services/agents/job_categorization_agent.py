from langchain_ollama import ChatOllama
from langchain_core.prompts import (
    SystemMessagePromptTemplate,
    HumanMessagePromptTemplate,
    ChatPromptTemplate,
)
from app.core.config import settings
from langchain_core.output_parsers import JsonOutputParser
from langchain_core.runnables import RunnableMap

llm = ChatOllama(model=settings.OLLAMA_MODEL, base_url=settings.OLLAMA_BASE_URL)

system = SystemMessagePromptTemplate.from_template(
    "You are an intelligent maintenance classification assistant."
)
question = HumanMessagePromptTemplate.from_template(
    "Given a job title and description, categorize the job according to:\n"
    "- Category: One of [Maintenance, Repair, Inspection, Cleaning, Landscaping, Emergency]\n"
    "- Priority: One of [Urgent, High, Routine, Low]\n"
    "- Status: One of [Pending, Assigned, InProgress, Completed, Cancelled, OnHold]\n\n"
    "Title: {job_title}\n"
    "Description: {job_description}\n\n"
    "Respond in JSON format:\n"
    "{{\n"
    '  "Category": "...",\n'
    '  "Priority": "...",\n'
    '  "Responsibility": "..."\n'
    "}}\n"
)
messages = [system, question]
template = ChatPromptTemplate(messages)

parser = JsonOutputParser()

categorization_agent = (
    RunnableMap(
        {
            "job_title": lambda x: x["title"],
            "job_description": lambda x: x["description"],
        }
    )
    | template
    | llm
    | parser
)