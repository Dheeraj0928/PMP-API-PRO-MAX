using Microsoft.AspNetCore.Mvc;
using Pmp.Api.Contracts;
using Pmp.Api.Models;
using Pmp.Api.Services;
using MyAwesomeTools;

namespace Pmp.Api.Controllers;

[ApiController ]
[Route("api/projects")]
public class ProjectControllers : ControllerBase
{
    private readonly IProjectService _service;
    private readonly ICalculator _calculator;

    public ProjectControllers(IProjectService service, ICalculator calculator)
    {
        _service = service;
        _calculator = calculator;
    }

    [HttpGet]
    public async  Task<IActionResult> GetProjects()
    {   
        var projects = await _service.GetAllProjectsAsync();
        return Ok("project is working!!");
    }

    [HttpGet("test-add")]
    public IActionResult TestAdd()
    {
        int result = _calculator.Add(15, 25);
        return Ok(new { Sum = result });
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