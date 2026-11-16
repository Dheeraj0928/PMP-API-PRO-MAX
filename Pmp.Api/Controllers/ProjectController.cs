using Microsoft.AspNetCore.Mvc;
using Pmp.Api.Contracts;
using Pmp.Api.Models;
using Pmp.Api.Services;

namespace Pmp.Api.Controllers;

[ApiController ]
[Route("api/projects")]
public class ProjectControllers : ControllerBase
{
    private readonly IProjectService _service;
    

    public ProjectControllers(IProjectService service)
    {
        _service = service;
        
    }

    [HttpGet]
    public async  Task<IActionResult> GetProjects()
    {   
        var projects = await _service.GetAllProjectsAsync();
        return Ok(projects);
    }

    
   

    [HttpPost]
    public async Task<IActionResult> CreatProject(
        [FromBody] CreateProjectRequest request)
    {
        var project= await _service.CreateProjectAsync(request);
        return StatusCode(201,new
        {
            Message = "project created sucessfullly",
            ProjectId=project.Id
        });
    }
}