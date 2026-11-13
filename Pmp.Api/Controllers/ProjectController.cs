using Microsoft.AspNetCore.Mvc;
using Pmp.Api.Contracts;
using Pmp.Api.Models;
using Pmp.Api.Services;
using MyAwesomeTools; // 👉 [ADDED]: Desktop wale custom package ka namespace import kiya

namespace Pmp.Api.Controllers;

[ApiController ]
[Route("api/projects")]
public class ProjectControllers : ControllerBase
{
    private readonly IProjectService _service;
    private readonly ICalculator _calculator; // 👉 [ADDED]: Desktop package ki interface field banayi

    // 👉 [ADDED]: Constructor mein ICalculator ko bhi dependency injection ke zariye le liya
    public ProjectControllers(IProjectService service, ICalculator calculator)
    {
        _service = service;
        _calculator = calculator; // 👉 [ADDED]: Assign kar diya
    }

    //Get api
    [HttpGet]
    public async  Task<IActionResult> GetProjects()
    {   
        var projects = await _service.GetAllProjectsAsync();
        return Ok("project is working!!");
    }

    // 👉 [ADDED]: Naya endpoint banaya desktop package ke function ko test karne ke liye
    [HttpGet("test-add")]
    public IActionResult TestAdd()
    {
        int result = _calculator.Add(15, 25); // Desktop wali file ka Add function call hua
        return Ok(new { Sum = result });
    }

    //post: Api/projects
    //To create new project
    [HttpPost]
    public async Task<IActionResult> CreatProject(
        [FromBody] CreateProjectRequest request)
    {
        // to create project 
        var project= await _service.CreateProjectAsync(request);
        return StatusCode(201,new
        {
            Message = "project created sucessfullly",
            ProjectId=project.Id
        });
    }
}