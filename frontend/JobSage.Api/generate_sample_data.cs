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
                jobs.Add(new Job
                {
                    Id = jobId,
                    Title = jobTitles[rnd.Next(jobTitles.Length)],
                    Description = jobDescriptions[rnd.Next(jobDescriptions.Length)],
                    JobType = (JobType)(j % Enum.GetValues(typeof(JobType)).Length),
                    Priority = (JobPriority)(j % Enum.GetValues(typeof(JobPriority)).Length),
                    Status = (JobStatus)(j % Enum.GetValues(typeof(JobStatus)).Length),
                    PropertyInfoId = propertyId,
                    SchedulingId = schedulingId,
                    Cost = new JobCost { EstimatedCost = rnd.Next(100, 1000), ActualCost = null, ApprovedBudget = rnd.Next(100, 1000) },
                    CreatedBy = Guid.NewGuid(),
                    AssignedTo = null,
                    TenantContact = "tenant@email.com"
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
    }
}
