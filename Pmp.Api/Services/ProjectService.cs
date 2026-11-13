
using Pmp.Api.Contracts;
using Pmp.Api.Models;
using Pmp.Api.Repositories;

namespace Pmp.Api.Services;

public class ProjectService(IProjectRepository repository) : IProjectService
{
    private readonly IProjectRepository _repository = repository;

    public async Task<bool> IsCodeAvailableAsync(string code)
    {
        bool exists = await _repository.CodeExistsAsync(code);

        return !exists;
    }

    public async Task<Project> CreateProjectAsync(
        CreateProjectRequest request)
    {

        if (string.IsNullOrWhiteSpace(request.Code))
        {
            throw new ArgumentException(
                "Project code is required.");
        }

        // 2. Check project name
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            throw new ArgumentException(
                "Project name is required.");
        }

        string code = request.Code.Trim();

        // 3. Check duplicate project code
        bool available = await IsCodeAvailableAsync(code);

        if (!available)
        {
            throw new InvalidOperationException(
                "Project code already exists.");
        }

        // 4. Check customer
        bool customerExists = await _repository
            .CustomerExistsAsync(request.CustomerId);

        if (!customerExists)
        {
            throw new ArgumentException(
                "Customer does not exist.");
        }

        // 5. Check onshore manager
        bool onshoreExists = await _repository
            .EmployeeExistsAsync(request.OnshoreManagerId);

        if (!onshoreExists)
        {
            throw new ArgumentException(
                "Onshore manager does not exist.");
        }

        // 6. Check offshore manager
        bool offshoreExists = await _repository
            .EmployeeExistsAsync(request.OffshoreManagerId);

        if (!offshoreExists)
        {
            throw new ArgumentException(
                "Offshore manager does not exist.");
        }

        // 7. Validate project dates
        if (request.EndDate < request.StartDate)
        {
            throw new ArgumentException(
                "End date cannot be before start date.");
        }

        // 8. Validate estimated hours
        if (request.EstimatedHours < 0)
        {
            throw new ArgumentException(
                "Estimated hours cannot be negative.");
        }

        // 9. Create project object
        var project = new Project
        {
            Code = code,
            Name = request.Name.Trim(),

            CustomerId = request.CustomerId,
            OnshoreManagerId = request.OnshoreManagerId,
            OffshoreManagerId = request.OffshoreManagerId,

            StartDate = request.StartDate,
            EndDate = request.EndDate,
            RevisedEndDate = request.RevisedEndDate,
            RenewalDate = request.RenewalDate,

            EstimatedHours = request.EstimatedHours,
            ActualHours = request.ActualHours,

            Status = request.Status,
            Domain = request.Domain,
            Type = request.Type,
            Sector = request.Sector,
            Methodology = request.Methodology,

            CreatedAt = DateTimeOffset.UtcNow
        };

        // 10. Save project
        await _repository.AddAsync(project);

        // Return saved project
        return project;
    }

    // Get all projects
    public async Task<List<Project>> GetAllProjectsAsync()
    {
        return await _repository.GetAllAsync();
    }
}
