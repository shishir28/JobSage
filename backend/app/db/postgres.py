from sqlalchemy import create_engine, MetaData, Table, select
from sqlalchemy.orm import sessionmaker
from app.core.config import settings


DATABASE_URL = (
    f"postgresql://{settings.POSTGRES_USER}:{settings.POSTGRES_PASSWORD}"
    f"@{settings.POSTGRES_HOST}:{settings.POSTGRES_PORT}/{settings.POSTGRES_DB}"
)

engine = create_engine(DATABASE_URL)
SessionLocal = sessionmaker(bind=engine)
metadata = MetaData()

def fetch_jobs_from_postgres():
    """Fetch all jobs from the PostgreSQL database using SQLAlchemy."""
    jobs_table = Table("Jobs", metadata, autoload_with=engine)
    with SessionLocal() as session:
        stmt = select(jobs_table)
        result = session.execute(stmt)
        jobs = [dict(row._mapping) for row in result.fetchall()]
    return jobs