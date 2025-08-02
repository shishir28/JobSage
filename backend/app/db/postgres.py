import psycopg2
from psycopg2.extras import DictCursor
from app.core.config import settings

POSTGRES_CONFIG = {
    "dbname": settings.POSTGRES_DB,
    "user": settings.POSTGRES_USER,
    "password": settings.POSTGRES_PASSWORD,
    "host": settings.POSTGRES_HOST,
    "port": settings.POSTGRES_PORT,
}

def fetch_jobs_from_postgres():
    """Fetch all jobs from the PostgreSQL database."""
    connection = psycopg2.connect(**POSTGRES_CONFIG)
    cursor = connection.cursor(cursor_factory=DictCursor)
    cursor.execute('SELECT * FROM "Jobs";')
    jobs = cursor.fetchall()
    cursor.close()
    connection.close()
    return jobs