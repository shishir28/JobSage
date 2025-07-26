namespace JobSage.Domain.Enums
{
    public enum JobType
    {
        Maintenance,
        Repair,
        Inspection,
        Cleaning,
        Landscaping,
        Emergency
    }

    public enum JobPriority
    {
        Low,
        Medium,
        High,
        Urgent
    }

    public enum JobStatus
    {
        Pending,
        Assigned,
        InProgress,
        Completed,
        Cancelled,
        OnHold
    }
}
