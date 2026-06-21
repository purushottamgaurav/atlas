namespace DotNetMvc.Models;

public enum JobType
{
    FullTime,
    PartTime,
    Contract,
    Internship,
    Remote
}

public enum ApplicationStatus
{
    Pending,
    UnderReview,
    Accepted,
    Rejected
}

public enum UserRole
{
    Employer,
    JobSeeker
}
