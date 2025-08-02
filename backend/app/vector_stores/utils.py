from langchain_openai import OpenAIEmbeddings

embeddings = OpenAIEmbeddings()

def generate_embedding(text, model="text-embedding-ada-002"):
    """Generate an embedding for the given text."""
    return embeddings.embed_query(text, model=model)
