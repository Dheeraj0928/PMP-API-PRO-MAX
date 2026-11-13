using Pmp.Api.Models;

namespace Pmp.Api.Repositories;

public interface IProjectRepository
{
    Task<bool> CodeExistsAsync(string code);
    Task<bool> CustomerExistsAsync(int customerId);
    Task<bool> EmployeeExistsAsync(int employeeId);
    Task<List<Project>> GetAllAsync();
    Task AddAsync(Project project);
}