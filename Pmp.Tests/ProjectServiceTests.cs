using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Pmp.Api.Contracts;
using Pmp.Api.Data;
using Pmp.Api.Models;
using Pmp.Api.Services;

namespace Pmp.Tests;

public class ProjectServicesTest
{
    [Fact]
    public async Task DuplicateProjectCode_ShouldRejected()
    {
        await using var context = CreateSeededContext();
        var service = new ProjectService(context);
        var request = new CreateProjectRequest
        {
            Code = "EXISTING-PROJECT",
            Name = "Duplicate Project"
        };

        Func<Task> act = () => service.CreateProjectAsync(request);

        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task Invalidcustomer_ShouldRejectProject()
    {
        await using var context = CreateSeededContext();
        var service = new ProjectService(context);
        var request = new CreateProjectRequest
        {
            Code = "Test-Invalid-Customer",
            Name = "Test Project",
            CustomerId = -1
        };

        await Assert.ThrowsAsync<ArgumentException>(
            async () => await service.CreateProjectAsync(request));
    }

    [Fact]
    public async Task GetAllProjects_ShouldReturnProjects()
    {
        await using var context = CreateSeededContext();
        var service = new ProjectService(context);

        var projects = await service.GetAllProjectsAsync();

        projects.Should().ContainSingle(project => project.Code == "EXISTING-PROJECT");
    }

    private static AppDbContext CreateSeededContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        var context = new AppDbContext(options);

        context.Customers.Add(new Customer
        {
            Id = 1,
            CustomerName = "Test Customer",
            CreatedAt = DateTimeOffset.UtcNow
        });
        context.Employees.AddRange(
            new Employee { Id = 1, Name = "Onshore Manager" },
            new Employee { Id = 2, Name = "Offshore Manager" });
        context.Projects.Add(new Project
        {
            Code = "EXISTING-PROJECT",
            Name = "Existing Project",
            CustomerId = 1,
            OnshoreManagerId = 1,
            OffshoreManagerId = 2,
            CreatedAt = DateTimeOffset.UtcNow
        });
        context.SaveChanges();

        return context;
    }

    
}
