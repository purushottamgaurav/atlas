using DotNetMvc.Models;

namespace DotNetMvc.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        if (context.Jobs.Any()) return;

        var jobs = new List<Job>
        {
            new() { Title = "Senior .NET Developer", Company = "TechCorp", Location = "Seattle, WA", JobType = JobType.FullTime, Description = "Build enterprise applications using .NET 10 and Azure. You will work with a talented team designing scalable microservices and cloud-native solutions.", Requirements = "5+ years .NET experience, Azure cloud, C#, SQL Server, REST APIs", SalaryRange = "$120,000 - $150,000", PostedByUserId = "seed-employer", PostedDate = DateTime.UtcNow.AddDays(-5), IsActive = true },
            new() { Title = "React Frontend Engineer", Company = "StartupXYZ", Location = "Remote", JobType = JobType.Remote, Description = "Join our fast-growing startup building next-gen UI experiences. You will own the frontend architecture and collaborate closely with designers.", Requirements = "3+ years React, TypeScript, CSS-in-JS, REST/GraphQL, Figma", SalaryRange = "$90,000 - $120,000", PostedByUserId = "seed-employer", PostedDate = DateTime.UtcNow.AddDays(-3), IsActive = true },
            new() { Title = "DevOps Engineer", Company = "CloudBase Inc", Location = "Austin, TX", JobType = JobType.FullTime, Description = "Manage CI/CD pipelines and Azure infrastructure for our SaaS platform serving millions of users.", Requirements = "Kubernetes, Terraform, Azure DevOps, GitHub Actions, Linux", SalaryRange = "$110,000 - $135,000", PostedByUserId = "seed-employer", PostedDate = DateTime.UtcNow.AddDays(-7), IsActive = true },
            new() { Title = "UX Designer", Company = "DesignHub", Location = "New York, NY", JobType = JobType.Contract, Description = "Design intuitive user experiences for our suite of web and mobile products. You will run user research, create wireframes, and deliver pixel-perfect prototypes.", Requirements = "Figma, user research, usability testing, design systems, mobile UX", SalaryRange = "$75/hr", PostedByUserId = "seed-employer", PostedDate = DateTime.UtcNow.AddDays(-2), IsActive = true, ApplicationDeadline = DateTime.UtcNow.AddDays(28) },
            new() { Title = "Data Analyst Intern", Company = "DataDriven Co", Location = "Chicago, IL", JobType = JobType.Internship, Description = "Analyze large datasets and build dashboards that drive business decisions. Great opportunity to learn from senior analysts in a data-first company.", Requirements = "Python or R, SQL, Excel, basic statistics, eagerness to learn", SalaryRange = "$25/hr", PostedByUserId = "seed-employer", PostedDate = DateTime.UtcNow.AddDays(-1), IsActive = true },
            new() { Title = "Product Manager", Company = "GrowthLabs", Location = "San Francisco, CA", JobType = JobType.FullTime, Description = "Define the product roadmap and work closely with engineering, design, and marketing teams to ship features customers love.", Requirements = "3+ years PM experience, Agile/Scrum, data-driven mindset, excellent communication", SalaryRange = "$130,000 - $160,000", PostedByUserId = "seed-employer", PostedDate = DateTime.UtcNow.AddDays(-4), IsActive = true },
            new() { Title = "Python ML Engineer", Company = "AI Ventures", Location = "Remote", JobType = JobType.Remote, Description = "Build and deploy machine learning models at scale. You will work on NLP, recommendation systems, and computer vision projects.", Requirements = "Python, PyTorch or TensorFlow, MLOps, Docker, cloud ML platforms", SalaryRange = "$140,000 - $180,000", PostedByUserId = "seed-employer", PostedDate = DateTime.UtcNow.AddDays(-6), IsActive = true },
            new() { Title = "QA Automation Engineer", Company = "QualityFirst", Location = "Denver, CO", JobType = JobType.FullTime, Description = "Build automated test suites using Selenium and Playwright for our e-commerce platform. You will define quality standards and mentor junior testers.", Requirements = "Selenium, Playwright, C# or Python, API testing, CI integration", SalaryRange = "$85,000 - $105,000", PostedByUserId = "seed-employer", PostedDate = DateTime.UtcNow.AddDays(-8), IsActive = true },
            new() { Title = "Technical Writer", Company = "DocuSoft", Location = "Remote", JobType = JobType.PartTime, Description = "Write developer-facing API documentation, tutorials, and getting-started guides. You will work with engineers to translate complex features into clear prose.", Requirements = "Technical writing, Markdown, REST API knowledge, developer empathy, Git", SalaryRange = "$45/hr", PostedByUserId = "seed-employer", PostedDate = DateTime.UtcNow.AddDays(-9), IsActive = true },
            new() { Title = "Cloud Architect", Company = "Enterprise Solutions", Location = "Boston, MA", JobType = JobType.FullTime, Description = "Design multi-region Azure architectures for Fortune 500 clients. You will lead technical discovery workshops and produce reference architectures.", Requirements = "Azure Solutions Architect certification, 8+ years cloud experience, IaC, networking", SalaryRange = "$160,000 - $200,000", PostedByUserId = "seed-employer", PostedDate = DateTime.UtcNow.AddDays(-10), IsActive = true },
        };

        context.Jobs.AddRange(jobs);
        await context.SaveChangesAsync();
    }
}
