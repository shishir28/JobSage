from sqlalchemy.orm import sessionmaker
from app.db.models import engine
from app.db.models.contractor import Contractor

SessionLocal = sessionmaker(autocommit=False, autoflush=False, bind=engine)

def get_contractor_details_from_db(contractorid: str):
    with SessionLocal() as session:
        contractor = session.query(Contractor).get(contractorid)
        if not contractor:
            return {}
        return {
            "name": contractor.Name,
            "trade": contractor.Trade,  # Added trade
            "rating": contractor.Rating,
            "availability": contractor.Availability,  # Added availability
            "contactinfo": contractor.ContactInfo,
            "location": contractor.Location,
            "hourlyrate": contractor.HourlyRate,
            "preferred": contractor.Preferred,  # Added preferred status
            "warrantyApproved": contractor.WarrantyApproved  # Added warranty approval
        }
