
using Xunit;
using FluentAssertions;
using Moq;
using Pmp.Api.Repositories;
using Pmp.Api.Contracts;
using Pmp.Api.Models;
using Pmp.Api.Services;
using Azure.Core.Pipeline;

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
        // Step 1: Fake repository create 
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

    [Fact]
    public async Task EmptyProjectCode_ShouldBeRejected()
    {
        // Given
        var mockRepo = new Mock<IProjectRepository>();
        var service = new ProjectService(mockRepo.Object);

        var request = new CreateProjectRequest
        {
            Code = "",
            Name = "Website Project"
        };

        // When
        Func<Task> act = () => service.CreateProjectAsync(request);

        // Then
        await act.Should().ThrowAsync<ArgumentException>().WithMessage("Project code is required.");

        mockRepo.Verify(
            repo => repo.AddAsync(It.IsAny<Project>()), Times.Never
        );
    }
    [Fact]
    public async Task WhitespaceProjectCode_ShouldBeRejected()
    {
        // Given
        var mockRepo = new Mock<IProjectRepository>();
        var service = new ProjectService(mockRepo.Object);
        var request = new CreateProjectRequest
        {
            Code = "",
            Name = "Website Project"
        };

        // When
        Func<Task> act = () => service.CreateProjectAsync(request);
        // Then
        await act.Should().ThrowAsync<ArgumentException>().WithMessage("Project code is required.");
        mockRepo.Verify(
            repo => repo.AddAsync(It.IsAny<Project>()),
            Times.Never
        );
    }

    [Fact]
    public async Task EmptyProjectName_ShouldBeRejected()
    {
        // Given

        var mockRepo = new Mock<IProjectRepository>();
        var service = new ProjectService(mockRepo.Object);
        var request = new CreateProjectRequest
        {
            Code = "PRj007",
            Name = ""
        };

        // When
        Func<Task> act = () => service.CreateProjectAsync(request);
        // Then
        await act.Should().ThrowAsync<ArgumentException>().WithoutMessage("Project name is required");

        mockRepo.Verify(
            repo => repo.AddAsync(It.IsAny<Project>()),Times.Never
        );
    }
    [Fact]
    public async Task WhitespaceProjectName_ShouldBeRejected()
    {
        // Given
        var mockRepo = new Mock<IProjectRepository>();
        var service = new ProjectService(mockRepo.Object);
        var request = new CreateProjectRequest
        {
            Code = "PRJ008",
            Name = ""

        };
        // When

        Func<Task> act = () => service.CreateProjectAsync(request);

        // Then

        await act.Should().ThrowAsync<ArgumentException>().WithMessage("Project name is required.");
        mockRepo.Verify(
            repo => repo.AddAsync(It.IsAny<Project>()),Times.Never
        );

    }
    [Fact]
    public async Task InvalidOnshoreManager_ShouldRejectProject()
    {
        // Given
        var mockRepo = new Mock<IProjectRepository>();

        mockRepo.Setup(repo => repo.CustomerExistsAsync(1)).ReturnsAsync(true);

        mockRepo.Setup(repo => repo.EmployeeExistsAsync(-1)).ReturnsAsync(false);

        var service = new ProjectService(mockRepo.Object);
        var request = new CreateProjectRequest
        {
            Code = "PRJ009",
            Name = "Website Project"
        };

        // When

        Func<Task> act = () => service.CreateProjectAsync(request);

        // Then
        await act.Should().ThrowAsync<ArgumentException>().WithMessage("Customer does not exist.");
        mockRepo.Verify(
            repo => repo.AddAsync(It.IsAny<Project>()), Times.Never
        );
    }
    [Fact]
    public async Task InvalidOffshoreManager_ShouldRejectProject()
    {
        // Given

        var mockRepo = new Mock<IProjectRepository>();

        mockRepo.Setup(repo => repo.CustomerExistsAsync(1)).ReturnsAsync(true);
        mockRepo.Setup(repo => repo.EmployeeExistsAsync(2)).ReturnsAsync(true);
        mockRepo.Setup(repo => repo.EmployeeExistsAsync(-1)).ReturnsAsync(false);

        var service = new ProjectService(mockRepo.Object);
        var request = new CreateProjectRequest
        {
            Code = "PRJ010",
            Name = "Mobile App Project",
            CustomerId = 1,
            OnshoreManagerId = 2,
            OffshoreManagerId = -1

        };


        // When
        Func<Task> act = () => service.CreateProjectAsync(request);
        // Then

        await act.Should().ThrowAsync<ArgumentException>().WithMessage("Offshore manager does not exist.");

        mockRepo.Verify(
            repo => repo.AddAsync(It.IsAny<Project>()), Times.Never
        );
    }

    [Fact]
    public async Task EndDateBeforeStartDate_ShouldRejectProject()
    {
        // Given

        var mockRepo = new Mock<IProjectRepository>();
        mockRepo.Setup(repo => repo.CustomerExistsAsync(1)).ReturnsAsync(true);
        mockRepo.Setup(repo => repo.EmployeeExistsAsync(1)).ReturnsAsync(true);
        mockRepo.Setup(repo => repo.EmployeeExistsAsync(2)).ReturnsAsync(true);

        var service = new ProjectService(mockRepo.Object);
        var request = new CreateProjectRequest
        {
            Code = "PRJ011",
            Name = "Website Project",
            CustomerId = 1,
            OnshoreManagerId = 1,
            OffshoreManagerId = 2,
            StartDate = new DateTimeOffset(
                2026, 11, 20, 0, 0, 0, TimeSpan.Zero
            ),
            EndDate = new DateTimeOffset(
                2026, 11, 10, 0, 0, 0, TimeSpan.Zero
            )
        };
        // When
        Func<Task> act = () => service.CreateProjectAsync(request);

        // Then
        await act.Should().ThrowAsync<ArgumentException>().WithMessage("End date cannot be before start date.");
        mockRepo.Verify(repo => repo.AddAsync(It.IsAny<Project>()), Times.Never);

    }

    [Fact]
    public async Task ProjectName_ShouldBeTrimed()
    {
        //Arrange
        var mockRepo= new Mock<IProjectRepository>();
        mockRepo.Setup(repo=>repo.CustomerExistsAsync(1)).ReturnsAsync(true);
        mockRepo.Setup(repo=>repo.EmployeeExistsAsync(1)).ReturnsAsync(true);
        mockRepo.Setup(repo=>repo.EmployeeExistsAsync(2)).ReturnsAsync(true);
        var service= new ProjectService(mockRepo.Object);
        var request = new CreateProjectRequest
        {
            Code="PRJ015",
            Name="  Mobile App  ",
            CustomerId=1,
            OnshoreManagerId=1,
            OffshoreManagerId=2,
            StartDate= new DateTimeOffset(2026,11,1,0,0,0, TimeSpan.Zero),
            EndDate= new DateTimeOffset(2026,12,2,0,0,0, TimeSpan.Zero)
        };
        //Act
        var project =await service.CreateProjectAsync(request);

        //Assert
        project.Name.Should().Be("Mobile App");
        mockRepo.Verify(
            repo=>repo.AddAsync(It.Is<Project>(p=> p.Name == "Mobile App")),Times.Once
        );
    }


[Fact]
public async Task NegativeEstimatedHours_ShouldRejectProject()
{
    // ARRANGE
    var mockRepo = new Mock<IProjectRepository>();

    mockRepo.Setup(repo => repo.CustomerExistsAsync(1))
            .ReturnsAsync(true);

    mockRepo.Setup(repo => repo.EmployeeExistsAsync(1))
            .ReturnsAsync(true);

    mockRepo.Setup(repo => repo.EmployeeExistsAsync(2))
            .ReturnsAsync(true);

    var service = new ProjectService(mockRepo.Object);

    var request = new CreateProjectRequest
    {
        Code = "PRJ012",
        Name = "Website Project",
        CustomerId = 1,
        OnshoreManagerId = 1,
        OffshoreManagerId = 2,
        StartDate = new DateTimeOffset(2026, 11, 1, 0, 0, 0, TimeSpan.Zero),
        EndDate = new DateTimeOffset(2026, 12, 1, 0, 0, 0, TimeSpan.Zero),
        EstimatedHours = -10
    };

    // ACT
    Func<Task> act = () => service.CreateProjectAsync(request);

    // ASSERT
    await act.Should()
        .ThrowAsync<ArgumentException>()
        .WithMessage("Estimated hours cannot be negative.");

    mockRepo.Verify(
        repo => repo.AddAsync(It.IsAny<Project>()),
        Times.Never);
}


[Fact]
public async Task ZeroEstimatedHours_ShouldBeAllowed()
{
    // ARRANGE
    var mockRepo = new Mock<IProjectRepository>();

    mockRepo.Setup(repo => repo.CustomerExistsAsync(1)).ReturnsAsync(true);

    mockRepo.Setup(repo => repo.EmployeeExistsAsync(1)).ReturnsAsync(true);

    mockRepo.Setup(repo => repo.EmployeeExistsAsync(2)).ReturnsAsync(true);

    var service = new ProjectService(mockRepo.Object);

    var request = new CreateProjectRequest
    {
        Code = "PRJ013",
        Name = "Website Project",
        CustomerId = 1,
        OnshoreManagerId = 1,
        OffshoreManagerId = 2,
        StartDate = new DateTimeOffset(2026, 11, 1, 0, 0, 0, TimeSpan.Zero),
        EndDate = new DateTimeOffset(2026, 12, 1, 0, 0, 0, TimeSpan.Zero),
        EstimatedHours = 0
    };

    // ACT
    var project = await service.CreateProjectAsync(request);

    // ASSERT
    project.EstimatedHours.Should().Be(0);

    mockRepo.Verify(
        repo => repo.AddAsync(It.IsAny<Project>()),Times.Once);
}


[Fact]
public async Task ProjectCode_ShouldBeTrimmed()
{
    // ARRANGE
    var mockRepo = new Mock<IProjectRepository>();

    mockRepo.Setup(repo => repo.CustomerExistsAsync(1)).ReturnsAsync(true);

    mockRepo.Setup(repo => repo.EmployeeExistsAsync(1)).ReturnsAsync(true);

    mockRepo.Setup(repo => repo.EmployeeExistsAsync(2)).ReturnsAsync(true);

    var service = new ProjectService(mockRepo.Object);

    var request = new CreateProjectRequest
    {
        Code = "  PRJ014  ",
        Name = "Website Project",
        CustomerId = 1,
        OnshoreManagerId = 1,
        OffshoreManagerId = 2,
        StartDate = new DateTimeOffset(2026, 11, 1, 0, 0, 0, TimeSpan.Zero),
        EndDate = new DateTimeOffset(2026, 12, 1, 0, 0, 0, TimeSpan.Zero)
    };

    // ACT
    var project = await service.CreateProjectAsync(request);

    // ASSERT
    project.Code.Should().Be("PRJ014");

    mockRepo.Verify(
        repo => repo.CodeExistsAsync("PRJ014"),
        Times.Once);

    mockRepo.Verify(
        repo => repo.AddAsync(It.Is<Project>(p => p.Code == "PRJ014")),Times.Once);
}
    [Fact]
public async Task GetAllProjects_WhenNoProjects_ShouldReturnEmptyList()
{
    // ARRANGE
    var mockRepo = new Mock<IProjectRepository>();

    mockRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(new List<Project>());

    var service = new ProjectService(mockRepo.Object);

    // ACT
    var projects = await service.GetAllProjectsAsync();

    // ASSERT
    projects.Should().NotBeNull();
    projects.Should().BeEmpty();

    mockRepo.Verify(repo => repo.GetAllAsync(),Times.Once);
}

[Fact]
public async Task DuplicateCodeWithSpaces_ShouldRejectProject()
    {
        //Arrange
        var mockRepo= new Mock<IProjectRepository>();
        mockRepo.Setup(repo=>repo.CodeExistsAsync("PRJ001")).ReturnsAsync(true);
        var service = new ProjectService(mockRepo.Object);
        var request = new CreateProjectRequest{
            Code="PRJ001",
            Name="Duplicate Website Project"
        };

        //Act

        Func<Task> act= ()=>service.CreateProjectAsync(request);
        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>().WithMessage("Project code already exists.");
        mockRepo.Verify(repo=>repo.CodeExistsAsync("PRJ001"),Times.Once);
    }

    [Fact]
    public async Task InvalidCustomer_ShouldStopFurtherProcessing()
    {
        var mockRepo= new Mock<IProjectRepository>();
        mockRepo.Setup(repo=>repo.CodeExistsAsync("PRJ018")).ReturnsAsync(false);
        mockRepo.Setup(repo=>repo.CustomerExistsAsync(999)).ReturnsAsync(false);
        var service = new ProjectService(mockRepo.Object);
        var request = new CreateProjectRequest
        {
            Code="PRJ018",
            Name="Customer Project",
            OnshoreManagerId=1,
            OffshoreManagerId=2
        };

        //Act
        Func<Task> act = ()=> service.CreateProjectAsync(request);

        //Assert
        await act.Should().ThrowAsync<ArgumentException>().WithMessage("Customer does not exist.");

        mockRepo.Verify(
            repo=>repo.AddAsync(It.IsAny<Project>()),Times.Never
        );

    }
    [Fact]
public async Task RepositorySaveFailure_ShouldPropagateException()
{
    // ARRANGE
    var mockRepo = new Mock<IProjectRepository>();

    mockRepo.Setup(repo => repo.CodeExistsAsync("PRJ019")).ReturnsAsync(false);

    mockRepo.Setup(repo => repo.CustomerExistsAsync(1)).ReturnsAsync(true);

    mockRepo.Setup(repo => repo.EmployeeExistsAsync(1)).ReturnsAsync(true);

    mockRepo.Setup(repo => repo.EmployeeExistsAsync(2)).ReturnsAsync(true);

    // Fake database save failure
    mockRepo.Setup(repo => repo.AddAsync(It.IsAny<Project>())).ThrowsAsync(new InvalidOperationException("Database save failed."));

    var service = new ProjectService(mockRepo.Object);

    var request = new CreateProjectRequest
    {
        Code = "PRJ019",
        Name = "New Project",
        CustomerId = 1,
        OnshoreManagerId = 1,
        OffshoreManagerId = 2,
        StartDate = new DateTimeOffset(2026, 11, 1, 0, 0, 0, TimeSpan.Zero),
        EndDate = new DateTimeOffset(2026, 12, 1, 0, 0, 0, TimeSpan.Zero),
        EstimatedHours = 100
    };

    // ACT
    Func<Task> act = () => service.CreateProjectAsync(request);

    // ASSERT
    await act.Should().ThrowAsync<InvalidOperationException>().WithMessage("Database save failed.");

    mockRepo.Verify(repo => repo.AddAsync(It.IsAny<Project>()),Times.Once);
    }

    [Fact]
    public async Task CreateProject_ShouldMapAllImportantFieldsCorrectly()
    {
        // Given
        var mockRepo = new Mock<IProjectRepository>();

        mockRepo.Setup(repo=>repo.CustomerExistsAsync(10)).ReturnsAsync(true);
        mockRepo.Setup(repo=>repo.EmployeeExistsAsync(20)).ReturnsAsync(true);
        mockRepo.Setup(repo=>repo.EmployeeExistsAsync(30)).ReturnsAsync(true);

        var service = new ProjectService(mockRepo.Object);
        var startDate = new DateTimeOffset(2026,11,1,0,0,0,TimeSpan.Zero);
        var endDate =  new DateTimeOffset(2027,1,1,0,0,0,TimeSpan.Zero);

        var request= new CreateProjectRequest
        {
            Code="PRJ020",
            Name= "Enterprice Dashboard",
            CustomerId=10,
            OnshoreManagerId=20,
            OffshoreManagerId=30,
            StartDate=startDate,
            EndDate=endDate,
            EstimatedHours= 250 ,
            ActualHours=50
        };
    
        // When
        var project = await service.CreateProjectAsync(request);
        // Then
        project.Code.Should().Be(request.Code);
        project.Name.Should().Be(request.Name);
        project.CustomerId.Should().Be(request.CustomerId);
        project.OnshoreManagerId.Should().Be(20);
        project.OffshoreManagerId.Should().Be(30);
        project.StartDate.Should().Be(startDate);
        project.EndDate.Should().Be(endDate);
        project.EstimatedHours.Should().Be(250);
        project.ActualHours.Should().Be(50);

        mockRepo.Verify(
            repo=>repo.AddAsync(It.Is<Project>(p=>
            p.Code=="PRJ020" && 
            p.CustomerId == 10 && 
            p.OnshoreManagerId == 20 &&
            p.OffshoreManagerId == 30 &&
            p.EstimatedHours == 250 &&
            p.ActualHours == 50)),Times.Once
        );
    }

    [Fact]
    public async Task CreateProject_ShouldCallRepositorySaveOnlyOnce ()
    {
        //Arrange
        var mockRepo = new Mock<IProjectRepository>();
        mockRepo.Setup(repo=>repo.CustomerExistsAsync(1)).ReturnsAsync(true);
        mockRepo.Setup(repo=>repo.EmployeeExistsAsync(1)).ReturnsAsync(true);
        mockRepo.Setup(repo=>repo.EmployeeExistsAsync(2)).ReturnsAsync(true);
    
        var service = new ProjectService(mockRepo.Object);
        var request = new CreateProjectRequest
        {
            Code="PRJ021",
            Name="New Project",
            CustomerId=1,
            OnshoreManagerId=1,
            OffshoreManagerId=2,
            StartDate= new DateTimeOffset(2026,11,1,0,0,0,TimeSpan.Zero),
            EndDate= new DateTimeOffset(2026,12,1,0,0,0,TimeSpan.Zero)
        };
        // Act
        await service.CreateProjectAsync(request);

        //Assert
        mockRepo.Verify(
            repo=>repo.AddAsync(It.IsAny<Project>()),
            Times.Once);
    }

    [Fact]
public async Task GetAllProjects_WhenRepositoryFails_ShouldThrowException()
{
    // ARRANGE
    var mockRepo = new Mock<IProjectRepository>();

    mockRepo.Setup(repo => repo.GetAllAsync()).ThrowsAsync(new InvalidOperationException("Unable to retrieve projects."));

    var service = new ProjectService(mockRepo.Object);

    // ACT
    Func<Task> act = () => service.GetAllProjectsAsync();

    // ASSERT
    await act.Should().ThrowAsync<InvalidOperationException>().WithMessage("Unable to retrieve projects.");

    mockRepo.Verify(
        repo => repo.GetAllAsync(),
        Times.Once);
}
}


