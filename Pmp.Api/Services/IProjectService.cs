using Pmp.Api.Contracts;
using Pmp.Api.Models;
namespace Pmp.Api.Services;

public interface IProjectService
{
    Task<bool> IsCodeAvailableAsync(string code);
    Task<Project> CreateProjectAsync(CreateProjectRequest request);
    Task<List<Project>> GetAllProjectsAsync();
}