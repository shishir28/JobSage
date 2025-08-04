
from . import db
from app.db.models.base import BaseModel



class Contractor(BaseModel):
    """Contractor model for storing contractor details in a SQL database."""
    __tablename__ = "Contractors"
    Id = db.Column(db.String, primary_key=True)
    Name = db.Column(db.Text, nullable=False)
    Trade = db.Column(db.Text, nullable=False)
    Rating = db.Column(db.Float, nullable=False)
    Availability = db.Column(db.Text, nullable=False)
    ContactInfo = db.Column(db.Text, nullable=False)
    Location = db.Column(db.Text, nullable=False)
    HourlyRate = db.Column(db.Float, nullable=False)
    Preferred = db.Column(db.Boolean, nullable=False)
    WarrantyApproved = db.Column(db.Boolean, nullable=False)

    def as_dict(self):
        """Convert the SQL contractor to a dictionary format."""
        return {
            "Id": self.Id,
            "Name": self.Name,
            "Trade": self.Trade,
            "Rating": self.Rating,
            "Availability": self.Availability,
            "ContactInfo": self.ContactInfo,
            "Location": self.Location,
            "HourlyRate": self.HourlyRate,
            "Preferred": self.Preferred,
            "WarrantyApproved": self.WarrantyApproved
        }
