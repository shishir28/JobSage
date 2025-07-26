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
   "You are a helpful assistant that summarizes maintenance job requests."
)
question = HumanMessagePromptTemplate.from_template(
    "Given the title and description of a maintenance job, generate a short and clear summary (1-2 sentences) of the task.\n"
    "Title: {job_title}\n"
    "Description: {job_description}\n\n"
    "Respond in JSON format:\n"
    "{{\n"
    '  "Summary": "..."'
    "}}\n"
)
messages = [system, question]
template = ChatPromptTemplate(messages)

parser = JsonOutputParser()

summarization_agent = (
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