
using Xunit;
using FluentAssertions;
using Moq;
using Pmp.Api.Repositories;
using Pmp.Api.Contracts;
using Pmp.Api.Models;
using Pmp.Api.Services;

namespace Pmp.Tests;

public class ProjectServicesTest
{
    // TEST 1: Duplicate project code reject hona chahiye
    [Fact]
    public async Task DuplicateProjectCode_ShouldRejected()
    {
        var mockRepo = new Mock<IProjectRepository>();

        mockRepo.Setup(repo => repo.CodeExistsAsync("EXISTING-PROJECT"))
                .ReturnsAsync(true);

        var service = new ProjectService(mockRepo.Object);

        var request = new CreateProjectRequest
        {
            Code = "EXISTING-PROJECT",
            Name = "Duplicate Project"
        };

        Func<Task> act = () => service.CreateProjectAsync(request);

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Project code already exists.");

        mockRepo.Verify(
            repo => repo.AddAsync(It.IsAny<Project>()),
            Times.Never);
    }

    // TEST 2: Invalid customer reject hona chahiye
    [Fact]
    public async Task Invalidcustomer_ShouldRejectProject()
    {
        var mockRepo = new Mock<IProjectRepository>();

        mockRepo.Setup(repo => repo.CodeExistsAsync("TEST-001"))
                .ReturnsAsync(false);

        mockRepo.Setup(repo => repo.CustomerExistsAsync(-1))
                .ReturnsAsync(false);

        var service = new ProjectService(mockRepo.Object);

        var request = new CreateProjectRequest
        {
            Code = "TEST-001",
            Name = "TEST Project",
            CustomerId = -1
        };

        Func<Task> act = () => service.CreateProjectAsync(request);

        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("Customer does not exist.");

        mockRepo.Verify(
            repo => repo.AddAsync(It.IsAny<Project>()),
            Times.Never);
    }

    // TEST 3: Saare projects return hone chahiye
    [Fact]
    public async Task GetAllProjects_ShouldReturnProjects()
    {
        var mockRepo = new Mock<IProjectRepository>();

        var fakeProjects = new List<Project>
        {
            new Project
            {
                Id = 1,
                Code = "PRJ001",
                Name = "Website Project"
            },
            new Project
            {
                Id = 2,
                Code = "PRJ002",
                Name = "Mobile App Project"
            }
        };

        mockRepo.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(fakeProjects);

        var service = new ProjectService(mockRepo.Object);

        var projects = await service.GetAllProjectsAsync();

        projects.Should().HaveCount(2);
        projects[0].Name.Should().Be("Website Project");
        projects[1].Name.Should().Be("Mobile App Project");

        mockRepo.Verify(
            repo => repo.GetAllAsync(),
            Times.Once);
    }

    
[Fact]
public async Task ValidProject_ShouldBeCreatedSuccessfully()
{
    // Step 1: Fake repository create karo
    var mockRepo = new Mock<IProjectRepository>();

    // Step 2: Project code already exist nahi karta
    mockRepo.Setup(repo => repo.CodeExistsAsync("PRJ003"))
            .ReturnsAsync(false);

    // Step 3: Customer exist karta hai
    mockRepo.Setup(repo => repo.CustomerExistsAsync(1))
            .ReturnsAsync(true);

    // Step 4: Onshore manager exist karta hai
    mockRepo.Setup(repo => repo.EmployeeExistsAsync(1))
            .ReturnsAsync(true);

    // Step 5: Offshore manager exist karta hai
    mockRepo.Setup(repo => repo.EmployeeExistsAsync(2))
            .ReturnsAsync(true);

    // Step 6: Service ko fake repository do
    var service = new ProjectService(mockRepo.Object);

    // Step 7: Valid project details
    var request = new CreateProjectRequest
    {
        Code = "PRJ003",
        Name = "Website Development",
        CustomerId = 1,
        OnshoreManagerId = 1,
        OffshoreManagerId = 2,

        StartDate = new DateTimeOffset(
            2026, 11, 1, 0, 0, 0, TimeSpan.Zero),

        EndDate = new DateTimeOffset(
            2026, 12, 1, 0, 0, 0, TimeSpan.Zero),

        EstimatedHours = 100
    };

    // Step 8: Project create karo
    var project = await service.CreateProjectAsync(request);

    // Step 9: Return hua project check karo
    project.Code.Should().Be("PRJ003");
    project.Name.Should().Be("Website Development");
    project.CustomerId.Should().Be(1);
    project.EstimatedHours.Should().Be(100);

    // Step 10: Confirm karo repository ne save method call kiya
    mockRepo.Verify(
        repo => repo.AddAsync(It.Is<Project>(p =>
            p.Code == "PRJ003" &&
            p.Name == "Website Development" &&
            p.CustomerId == 1)),
        Times.Once);
}

    
}
