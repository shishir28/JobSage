from typing import TypeVar, Type
from abc import abstractmethod
from . import db

T = TypeVar("T", bound="BaseModel")

class BaseModel(db.Model):
    __abstract__ = True

    @classmethod
    def create(cls: Type[T], commit=True,**kwargs) -> T:
        """Create a new instance of the model."""
        instance = cls(**kwargs)
        db.session.add(instance)
        db.session.commit()
        return instance

    @classmethod
    def delete(cls: Type[T], id: str) -> bool:
        """Delete an instance of the model."""
        instance = cls.query.get(id)
        if instance:
            db.session.delete(instance)
            db.session.commit()
            return True
        return False

    @classmethod
    def get_by_id(cls: Type[T], id: str) -> T | None:
        """Retrieve an instance of the model by its ID."""
        return cls.query.get(id)
