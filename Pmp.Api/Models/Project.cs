namespace Pmp.Api.Models;

public class Project
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;
    public int CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;

    public int OnshoreManagerId { get; set; }
    public Employee OnshoreManager { get; set; } = null!;
    public int OffshoreManagerId { get; set; }
    public Employee OffshoreManager { get; set; } = null!;
    public ProjectStatus Status { get; set; }

    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset EndDate { get; set; }
    public DateTimeOffset? RevisedEndDate { get; set; }
    public DateTimeOffset? RenewalDate { get; set; }
    public DateTimeOffset IssueDate { get; set; }
    public decimal EstimatedHours { get; set; }
    public decimal? ActualHours { get; set; }
    public DateTimeOffset CreatedAt { get; set; }

    public Domain Domain { get; set; }
    public ProjectType Type { get; set; }
    public Sector Sector { get; set; }
    public Methodology Methodology { get; set; }
    public List<Stakeholder> Stakeholders{get;set;}=[];
    public List<ProjectWorkstream> Workstreams{get;set;}=[];
}

public class Employee
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<Assignment> Assignments{get;set;}=[];

}

public class Customer
{
    public int Id { get; set; }
    public string CustomerName { get; set; } = string.Empty;

    public DateTimeOffset CreatedAt { get; set; }

    public List<Project> Projects { get; set; } = [];

}

public class ClientContact
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;
    public string ContactName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

}


public enum ProjectStatus
{
    PendingApproval,
    InProgress,
    Completed
}

public enum ProjectType
{
    Type1,
    Type2,

}
public enum Sector
{
    Automobile,
    Education,
    Business

}

public enum Methodology
{
    Scrum,
    Ep,
    TDD,

}

public enum Domain
{
    Domain1,
    Domain2,

}

public class Stakeholder
{
    public int ProjectId {get;set;} 
    public Project Project {get;set;} =null!;
    public int EmployeeId {get;set;} 
    public Employee Employee {get;set;} =null!;

}

public class ProjectWorkstream
{
    public int Id{get;set;}
    public int ProjectId{get;set;}
    public  Project Project{get;set;}=null!;

    public string StreamName{get;set;}=string.Empty;
    public List<Release> Releases{get;set;}=[];


}

public class Release
{
    public int Id{get;set;}
    public int ProjectWorkstreamId{get;set;}
    public  ProjectWorkstream  ProjectWorkstream{get;set;}=null!;

    public string ReleaseName{get;set;}=string.Empty;

    public List<Phase> Phases{get;set;}=[];


}
public class Phase
{
    public int Id{get;set;}
    public int ReleaseId{get;set;}
    public  Release Release{get;set;}=null!;

    public string PhaseName{get;set;}=string.Empty;
    public List<ProjectTask>ProjectTask{get;set;}=[];


}
public class ProjectTask
{
    public int Id{get;set;}
    public int PhaseId{get;set;}
    public  Phase Phase{get;set;}=null!;

    public string TaskName{get;set;}=string.Empty;
    public List<Effort> Efforts{get;set;}=[];
    public List<ProjectSubTask> ProjectSubTasks{get;set;}=[];
    public List<Assignment> Assignments{get;set;}=[];


}
public class Effort
{
    public int Id{get;set;}
    public int ProjectTaskId{get;set;}
    public  ProjectTask ProjectTask{get;set;}=null!;

    public string TaskName{get;set;}=string.Empty;


}
public class ProjectSubTask
{
    public int Id{get;set;}
    public int ProjectTaskId{get;set;}
    public  ProjectTask ProjectTask{get;set;}=null!;

    public string TaskName{get;set;}=string.Empty;


}

public class Assignment
{
    public int ProjectTaskId{get;set;}
    public ProjectTask ProjectTask{get;set;}=null!;
    public int EmployeeId{get;set;}
    public Employee Employee{get;set;}=null!;
}


