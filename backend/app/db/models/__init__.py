# JobSage AI Backend Application
from flask_sqlalchemy import SQLAlchemy
from sqlalchemy import create_engine
from sqlalchemy.orm import sessionmaker
from app.core.config import Settings

# Flask-SQLAlchemy instance for Flask routes
db = SQLAlchemy()
settings = Settings()

# Build SQLAlchemy DATABASE_URL from .env POSTGRES_* variables if DATABASE_URL is not set
DATABASE_URL = f"postgresql://{settings.POSTGRES_USER}:{settings.POSTGRES_PASSWORD}@{settings.POSTGRES_HOST}:{settings.POSTGRES_PORT}/{settings.POSTGRES_DB}"

# Plain SQLAlchemy engine and session for use outside Flask context
engine = create_engine(DATABASE_URL)
SessionLocal = sessionmaker(autocommit=False, autoflush=False, bind=engine)

# Import models to ensure they are registered with SQLAlchemy
from app.db.models.conversation import Conversation
from app.db.models.message import Message