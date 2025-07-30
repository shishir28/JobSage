using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using JobSage.Domain.Entities;
using JobSage.Domain.Enums;

public class SampleDataGenerator
{
    public static void GenerateSampleData()
    {
        var rnd = new Random();
        var jobTitles = new[] {
            "Emergency Plumbing - Burst Pipe", "Kitchen Sink Blockage Repair", "Bathroom Toilet Not Flushing", "Hot Water System Failure", "Electrical Safety Inspection",
            "Power Outlet Not Working", "Circuit Breaker Tripping", "Smoke Alarm Battery Replacement", "HVAC Filter Replacement", "Air Conditioning Not Cooling",
            "Heater Making Strange Noises", "Roof Tile Replacement", "Gutter Leak Repair", "Window Seal Deterioration", "Broken Glass Window Replacement",
            "Front Door Lock Replacement", "Garage Door Won't Open", "Fence Panel Blown Down", "Garden Sprinkler System Malfunction", "Tree Branch Removal",
            "Carpet Stain Removal", "Hardwood Floor Scratches", "Wall Paint Touch-up Required", "Kitchen Cabinet Door Hanging Loose", "Bathroom Grout Cleaning",
            "Pest Infestation - Ants", "Termite Inspection Required", "Security Camera Not Recording", "Intercom System Fault", "Pool Pump Not Working",
            "Pool Water Green/Cloudy", "Driveway Crack Sealing", "Parking Bay Line Repainting", "Common Area Light Replacement", "Stairwell Handrail Loose",
            "Lift Emergency Phone Not Working", "Fire Extinguisher Service Due", "Exit Sign Light Out", "Balcony Railing Safety Check", "Laundry Machine Error Code"
        };

        var jobDescriptions = new[] {
            "Tenant reports water gushing from pipe under kitchen sink. Requires immediate attention to prevent water damage.",
            "Kitchen sink completely blocked, water not draining. Tenant cannot use kitchen facilities.",
            "Toilet in unit 15B not flushing properly. Water level low and flush mechanism not working.",
            "No hot water in apartment 3A for 3 days. Gas hot water system may need repair or replacement.",
            "Annual electrical safety inspection due for entire building as per compliance requirements.",
            "Power outlet in bedroom not working. Tenant reports sparking sound when plugging in devices.",
            "Circuit breaker keeps tripping every time tenant uses microwave and toaster simultaneously.",
            "Smoke alarm in hallway beeping every 30 seconds indicating low battery needs replacement.",
            "HVAC system filter dirty and needs replacement. Reduced air flow reported by multiple tenants.",
            "Air conditioning unit in unit 7C not cooling despite being set to lowest temperature.",
            "Heating system making loud banging noises during operation. Possible loose component.",
            "Several roof tiles damaged after recent storm. Water potentially entering roof cavity.",
            "Gutter overflowing during rain, water pooling around building foundation.",
            "Window seals in living room deteriorated, allowing water and cold air to enter apartment.",
            "Large crack in bedroom window after hailstorm. Safety hazard and security concern.",
            "Front door lock mechanism jammed. Tenant cannot secure apartment properly.",
            "Electric garage door opener not responding to remote. Manual operation also difficult.",
            "Strong winds knocked down two fence panels. Privacy and security compromised.",
            "Automatic sprinkler system in garden area not turning on. Plants beginning to wilt.",
            "Large tree branch overhanging apartment balcony poses safety risk during storms.",
            "Red wine stain on common area carpet requires professional cleaning treatment.",
            "Deep scratches on hardwood floor in living room area need sanding and refinishing.",
            "Scuff marks and holes in hallway walls need patching and painting to maintain appearance.",
            "Kitchen cabinet door hanging crooked and won't close properly. Hinge may be broken.",
            "Bathroom tile grout discolored and showing signs of mold. Deep cleaning required.",
            "Ant infestation in kitchen area. Multiple entry points need to be identified and sealed.",
            "Annual termite inspection required as per building maintenance schedule.",
            "Security camera in parking area not recording footage. System needs diagnosis.",
            "Building intercom system crackling and cutting out during conversations.",
            "Pool circulation pump making grinding noise and not maintaining water clarity.",
            "Pool water turned green overnight. Chemical balance needs testing and adjustment.",
            "Multiple cracks appearing in concrete driveway. Sealing required before winter.",
            "Parking bay lines faded and barely visible. Repainting required for safety.",
            "LED light in common area stairwell flickering and needs replacement.",
            "Stairwell handrail coming loose from wall mounting. Safety hazard for residents.",
            "Emergency phone in lift not connecting to monitoring service during test call.",
            "Fire extinguisher in building corridor due for annual service and pressure test.",
            "Emergency exit sign light not illuminating. Battery backup may have failed.",
            "Balcony railing height and stability inspection required as per safety regulations.",
            "Washing machine displaying error code E3. Requires technician diagnosis and repair."
        };

        // Generate contractors first
        var contractors = GenerateContractors(rnd);

        var properties = new List<PropertyInformation>();
        var jobs = new List<Job>();
        var schedulingInfos = new List<SchedulingInfo>();

        // Australian addresses sample
        string[] streets = { "George St", "Elizabeth St", "Collins St", "Queen St", "King St", "Bourke St", "Pitt St", "Flinders St", "Oxford St", "St Kilda Rd" };
        string[] cities = { "Sydney", "Melbourne", "Brisbane", "Perth", "Adelaide", "Canberra", "Hobart", "Darwin" };
        string[] states = { "NSW", "VIC", "QLD", "WA", "SA", "ACT", "TAS", "NT" };

        for (int i = 0; i < 1000; i++)
        {
            var propertyId = Guid.NewGuid();
            var address = $"{rnd.Next(1, 200)} {streets[rnd.Next(streets.Length)]}, {cities[rnd.Next(cities.Length)]}, {states[rnd.Next(states.Length)]} {rnd.Next(2000, 3000)}";
            var property = new PropertyInformation
            {
                PropertyId = propertyId,
                PropertyAddress = address,
                UnitNumber = rnd.Next(1, 20).ToString(),
                LocationDetails = "Near park"
            };
            properties.Add(property);

            for (int j = 0; j < 4; j++)
            {
                var jobId = Guid.NewGuid();
                var schedulingId = Guid.NewGuid();
                var jobTitle = jobTitles[rnd.Next(jobTitles.Length)];
                var jobDescription = jobDescriptions[rnd.Next(jobDescriptions.Length)];

                // Assign contractor based on job type
                var assignedContractor = GetContractorForJob(jobTitle, jobDescription, contractors, rnd);

                jobs.Add(new Job
                {
                    Id = jobId,
                    Title = jobTitle,
                    Description = jobDescription,
                    JobType = (JobType)(j % Enum.GetValues(typeof(JobType)).Length),
                    Priority = (JobPriority)(j % Enum.GetValues(typeof(JobPriority)).Length),
                    Status = (JobStatus)(j % Enum.GetValues(typeof(JobStatus)).Length),
                    PropertyInfoId = propertyId,
                    SchedulingId = schedulingId,
                    Cost = new JobCost
                    {
                        EstimatedCost = rnd.Next(100, 1000),
                        ActualCost = null,
                        ApprovedBudget = rnd.Next(100, 1000)
                    },
                    CreatedBy = Guid.NewGuid(),
                    AssignedTo = null,
                    TenantContact = "tenant@email.com",
                    ContractorId = assignedContractor?.Id
                });
                schedulingInfos.Add(new SchedulingInfo
                {
                    SchedulingId = schedulingId,
                    CreatedAt = DateTime.UtcNow.AddDays(-rnd.Next(1, 365)),
                    UpdatedAt = DateTime.UtcNow,
                    DueDate = DateTime.UtcNow.AddDays(rnd.Next(1, 30)),
                    ScheduledDate = DateTime.UtcNow.AddDays(rnd.Next(1, 30)),
                    CompletedAt = null
                });
            }
        }

        File.WriteAllText("properties.json", JsonSerializer.Serialize(properties));
        File.WriteAllText("jobs.json", JsonSerializer.Serialize(jobs));
        File.WriteAllText("schedulinginfo.json", JsonSerializer.Serialize(schedulingInfos));
        File.WriteAllText("contractors.json", JsonSerializer.Serialize(contractors));
    }

    private static List<Contractor> GenerateContractors(Random rnd)
    {
        var contractors = new List<Contractor>();
        var contractorData = new[]
        {
            // Plumbing contractors
            new { Trade = "Plumbing", Names = new[] { "AquaFix Plumbing", "Rapid Response Plumbers", "Blue Water Solutions", "Metro Pipe Masters", "Elite Plumbing Services", "Emergency Plumbing Co", "Sydney Drain Experts", "Professional Plumbers Plus", "Leak Detection Specialists", "Hot Water Heroes" }, Rate = (60f, 85f) },

            // Electrical contractors
            new { Trade = "Electrical", Names = new[] { "PowerTech Electrical", "Bright Spark Electricians", "Current Solutions", "ElectroMax Services", "Lightning Fast Electric", "Circuit Pros", "Voltage Masters", "Wire Wizards", "Electric Excellence", "Safe Current Solutions" }, Rate = (70f, 95f) },

            // HVAC contractors
            new { Trade = "HVAC", Names = new[] { "Climate Control Experts", "AirFlow Masters", "Temperature Solutions", "Comfort Zone HVAC", "Cool Breeze Services", "Heat & Air Specialists", "Precision Climate", "Air Quality Pros", "Thermal Dynamics", "Environmental Controls" }, Rate = (65f, 90f) },

            // General Maintenance
            new { Trade = "General Maintenance", Names = new[] { "Handy Helper Services", "Fix-It-All Maintenance", "Property Care Specialists", "Maintenance Masters", "Quick Fix Solutions", "All Trades Maintenance", "Reliable Repair Services", "Property Preservation", "Maintenance Crew Plus", "Complete Care Services" }, Rate = (45f, 70f) },

            // Carpentry
            new { Trade = "Carpentry", Names = new[] { "Timber Craft Carpentry", "Precision Woodworks", "Custom Carpentry Co", "Wood Masters", "Cabinet Craft Experts", "Joinery Specialists", "Fine Finish Carpentry", "Traditional Timber Works", "Modern Wood Solutions", "Artisan Carpenters" }, Rate = (50f, 80f) },

            // Roofing
            new { Trade = "Roofing", Names = new[] { "SkyHigh Roofing", "Roof Rescue Services", "Summit Roofing Solutions", "Peak Performance Roofing", "Weatherproof Roofing", "Tile & Gutter Experts", "Storm Damage Specialists", "Rooftop Renovations", "Apex Roofing Services", "Guardian Roof Care" }, Rate = (55f, 85f) },

            // Painting
            new { Trade = "Painting", Names = new[] { "Color Perfect Painters", "Brush Masters", "Premium Paint Services", "Artistic Touch Painters", "Fresh Coat Specialists", "Interior Design Painters", "Wall Art Professionals", "Paint & Decor Experts", "Finish Line Painters", "Spectrum Painting" }, Rate = (40f, 65f) },

            // Flooring
            new { Trade = "Flooring", Names = new[] { "Floor Excellence", "Timber Floor Specialists", "Carpet & Tile Experts", "Surface Solutions", "Ground Level Flooring", "Premium Floor Care", "Floor Restoration Plus", "Hardwood Heroes", "Flooring Innovators", "Level Best Flooring" }, Rate = (45f, 75f) },

            // Pest Control
            new { Trade = "Pest Control", Names = new[] { "Bug Busters", "Pest Away Services", "Critter Control Experts", "Safe Pest Solutions", "Termite Terminators", "Eco Pest Management", "Professional Pest Control", "Insect Elimination", "Guardian Pest Services", "Green Pest Solutions" }, Rate = (35f, 60f) },

            // Security Systems
            new { Trade = "Security Systems", Names = new[] { "SecureGuard Systems", "Digital Security Solutions", "SafeWatch Technologies", "Advanced Security Systems", "Protection Plus", "Smart Security Specialists", "Surveillance Experts", "Access Control Pros", "Security Tech Solutions", "Guardian Systems" }, Rate = (60f, 100f) }
        };

        string[] locations = { "Sydney CBD", "North Sydney", "Parramatta", "Chatswood", "Bondi", "Manly", "Blacktown", "Liverpool", "Penrith", "Hornsby" };
        string[] availabilityOptions = { "Available", "Busy until next week", "Available after hours", "Emergency only", "Fully booked", "Available weekends" };

        int contractorId = 1;
        foreach (var tradeGroup in contractorData)
        {
            foreach (var name in tradeGroup.Names)
            {
                var rating = 3.5f + (float)(rnd.NextDouble() * 1.5); // Rating between 3.5 and 5.0
                var hourlyRate = tradeGroup.Rate.Item1 + (float)(rnd.NextDouble() * (tradeGroup.Rate.Item2 - tradeGroup.Rate.Item1));
                var preferred = rating >= 4.5f && rnd.NextDouble() < 0.7; // High-rated contractors more likely to be preferred
                var warrantyApproved = rating >= 4.0f && rnd.NextDouble() < 0.8;

                contractors.Add(new Contractor
                {
                    Id = $"contractor-{contractorId:D3}",
                    Name = name,
                    Trade = tradeGroup.Trade,
                    Rating = (float)Math.Round(rating, 1),
                    Availability = availabilityOptions[rnd.Next(availabilityOptions.Length)],
                    ContactInfo = $"{name.ToLower().Replace(" ", "").Replace("-", "")}@email.com, (02) {rnd.Next(8000, 9999)} {rnd.Next(1000, 9999)}",
                    Location = locations[rnd.Next(locations.Length)],
                    HourlyRate = (float)Math.Round(hourlyRate, 2),
                    Preferred = preferred,
                    WarrantyApproved = warrantyApproved
                });
                contractorId++;
            }
        }

        return contractors;
    }

    private static Contractor? GetContractorForJob(string jobTitle, string jobDescription, List<Contractor> contractors, Random rnd)
    {
        var jobTitleLower = jobTitle.ToLower();
        var jobDescriptionLower = jobDescription.ToLower();

        // Determine trade based on job content
        string? requiredTrade = null;

        if (jobTitleLower.Contains("plumb") || jobTitleLower.Contains("pipe") || jobTitleLower.Contains("sink") ||
            jobTitleLower.Contains("toilet") || jobTitleLower.Contains("water") || jobDescriptionLower.Contains("plumb") ||
            jobDescriptionLower.Contains("pipe") || jobDescriptionLower.Contains("drain") || jobDescriptionLower.Contains("water"))
        {
            requiredTrade = "Plumbing";
        }
        else if (jobTitleLower.Contains("electric") || jobTitleLower.Contains("power") || jobTitleLower.Contains("circuit") ||
                 jobTitleLower.Contains("outlet") || jobTitleLower.Contains("smoke alarm") || jobDescriptionLower.Contains("electric") ||
                 jobDescriptionLower.Contains("power") || jobDescriptionLower.Contains("circuit") || jobDescriptionLower.Contains("sparking"))
        {
            requiredTrade = "Electrical";
        }
        else if (jobTitleLower.Contains("hvac") || jobTitleLower.Contains("air conditioning") || jobTitleLower.Contains("heater") ||
                 jobTitleLower.Contains("heating") || jobDescriptionLower.Contains("hvac") || jobDescriptionLower.Contains("air conditioning") ||
                 jobDescriptionLower.Contains("cooling") || jobDescriptionLower.Contains("heating"))
        {
            requiredTrade = "HVAC";
        }
        else if (jobTitleLower.Contains("roof") || jobTitleLower.Contains("gutter") || jobDescriptionLower.Contains("roof") ||
                 jobDescriptionLower.Contains("gutter") || jobDescriptionLower.Contains("tiles"))
        {
            requiredTrade = "Roofing";
        }
        else if (jobTitleLower.Contains("door") || jobTitleLower.Contains("cabinet") || jobTitleLower.Contains("window") ||
                 jobDescriptionLower.Contains("door") || jobDescriptionLower.Contains("cabinet") || jobDescriptionLower.Contains("hinge"))
        {
            requiredTrade = "Carpentry";
        }
        else if (jobTitleLower.Contains("paint") || jobTitleLower.Contains("wall") || jobDescriptionLower.Contains("paint") ||
                 jobDescriptionLower.Contains("wall") || jobDescriptionLower.Contains("scuff"))
        {
            requiredTrade = "Painting";
        }
        else if (jobTitleLower.Contains("floor") || jobTitleLower.Contains("carpet") || jobDescriptionLower.Contains("floor") ||
                 jobDescriptionLower.Contains("carpet") || jobDescriptionLower.Contains("hardwood"))
        {
            requiredTrade = "Flooring";
        }
        else if (jobTitleLower.Contains("pest") || jobTitleLower.Contains("ant") || jobTitleLower.Contains("termite") ||
                 jobDescriptionLower.Contains("pest") || jobDescriptionLower.Contains("ant") || jobDescriptionLower.Contains("termite"))
        {
            requiredTrade = "Pest Control";
        }
        else if (jobTitleLower.Contains("security") || jobTitleLower.Contains("camera") || jobTitleLower.Contains("intercom") ||
                 jobDescriptionLower.Contains("security") || jobDescriptionLower.Contains("camera") || jobDescriptionLower.Contains("intercom"))
        {
            requiredTrade = "Security Systems";
        }
        else
        {
            requiredTrade = "General Maintenance";
        }

        // Get contractors for the required trade
        var availableContractors = contractors.Where(c => c.Trade == requiredTrade).ToList();

        if (!availableContractors.Any())
        {
            // Fallback to general maintenance
            availableContractors = contractors.Where(c => c.Trade == "General Maintenance").ToList();
        }

        if (!availableContractors.Any())
        {
            return null;
        }

        // Prefer high-rated, preferred contractors but also include some randomness
        var preferredContractors = availableContractors.Where(c => c.Preferred && c.Rating >= 4.5f).ToList();

        if (preferredContractors.Any() && rnd.NextDouble() < 0.6) // 60% chance to pick preferred
        {
            return preferredContractors[rnd.Next(preferredContractors.Count)];
        }
        else
        {
            return availableContractors[rnd.Next(availableContractors.Count)];
        }
    }
}
