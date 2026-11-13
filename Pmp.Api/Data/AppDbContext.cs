using Microsoft.EntityFrameworkCore;
using Pmp.Api.Models;

namespace Pmp.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Project> Projects => Set<Project>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<ClientContact> ClientContacts => Set<ClientContact>();
    public DbSet<Stakeholder> Stakeholder => Set<Stakeholder>();
    public DbSet<ProjectWorkstream> ProjectWorkstream => Set<ProjectWorkstream>();
    public DbSet<Release> Releases => Set<Release>();
    public DbSet<Phase> Phases => Set<Phase>();
    public DbSet<ProjectTask> ProjectTasks => Set<ProjectTask>();
    public DbSet<Effort> Efforts => Set<Effort>();
    public DbSet<ProjectSubTask> ProjectSubTasks => Set<ProjectSubTask>();
    public DbSet<Assignment> Assignments => Set<Assignment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasKey(project => project.Id);
            entity.Property(project => project.Code)
                .HasMaxLength(50)
                .IsRequired();
            entity.Property(project => project.Name)
                .HasMaxLength(200)
                .IsRequired();
            entity.Property(project => project.EstimatedHours).HasPrecision(18, 2);
            entity.Property(project => project.ActualHours).HasPrecision(18, 2);
            entity.Property(project => project.CreatedAt)
                .HasDefaultValueSql("SYSUTCDATETIME()");
            entity.Property(project => project.Domain)
                .HasConversion<string>()
                .HasMaxLength(30);
            entity.Property(project => project.Status)
                .HasConversion<string>()
                .HasMaxLength(30);
            entity.Property(project => project.Type)
                .HasConversion<string>()
                .HasMaxLength(30);
            entity.Property(project => project.Sector)
                .HasConversion<string>()
                .HasMaxLength(30);
            entity.Property(project => project.Methodology)
                .HasConversion<string>()
                .HasMaxLength(30);
            entity.HasIndex(project => project.Code).IsUnique();
            entity.HasOne(project => project.Customer)
                .WithMany(customer => customer.Projects)
                .HasForeignKey(project => project.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(project => project.OnshoreManager)
                .WithMany()
                .HasForeignKey(project => project.OnshoreManagerId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(project => project.OffshoreManager)
                .WithMany()
                .HasForeignKey(project => project.OffshoreManagerId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.Property(customer => customer.Id)
    .HasColumnName("CustomerId");
            entity.Property(customer => customer.CustomerName)
                .HasMaxLength(200)
                .IsRequired();
            entity.Property(customer => customer.CreatedAt)
                .HasDefaultValueSql("SYSUTCDATETIME()");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(employee => employee.Id);
            entity.Property(employee => employee.Name)
                .HasMaxLength(200)
                .IsRequired();
        });

        modelBuilder.Entity<ClientContact>(entity =>
        {
            entity.HasKey(contact => contact.Id);
            entity.Property(contact => contact.ContactName)
                .HasMaxLength(200)
                .IsRequired();
            entity.Property(contact => contact.Email)
                .HasMaxLength(320)
                .IsRequired();
            entity.HasOne(contact => contact.Customer)
                .WithMany()
                .HasForeignKey(contact => contact.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Stakeholder>(entity =>
        {
            entity.HasKey(stakeholder => new
            {
                stakeholder.ProjectId,
                stakeholder.EmployeeId
            });
            entity.HasOne(stakeholder => stakeholder.Project)
                .WithMany(project => project.Stakeholders)
                .HasForeignKey(stakeholder => stakeholder.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(stakeholder => stakeholder.Employee)
                .WithMany()
                .HasForeignKey(stakeholder => stakeholder.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<ProjectWorkstream>(entity =>
        {
            entity.HasKey(workstream => workstream.Id);
            entity.Property(workstream => workstream.StreamName)
                .HasMaxLength(200)
                .IsRequired();
            entity.HasOne(workstream => workstream.Project)
                .WithMany(project => project.Workstreams)
                .HasForeignKey(workstream => workstream.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Release>(entity =>
        {
            entity.HasKey(release => release.Id);
            entity.Property(release => release.ReleaseName)
                .HasMaxLength(200)
                .IsRequired();
            entity.HasOne(release => release.ProjectWorkstream)
                .WithMany(workstream => workstream.Releases)
                .HasForeignKey(release => release.ProjectWorkstreamId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Phase>(entity =>
        {
            entity.HasKey(phase => phase.Id);
            entity.Property(phase => phase.PhaseName)
                .HasMaxLength(200)
                .IsRequired();
            entity.HasOne(phase => phase.Release)
                .WithMany(release => release.Phases)
                .HasForeignKey(phase => phase.ReleaseId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<ProjectTask>(entity =>
        {
            entity.HasKey(projectTask => projectTask.Id);
            entity.Property(projectTask => projectTask.TaskName)
                .HasMaxLength(200)
                .IsRequired();
            entity.HasOne(projectTask => projectTask.Phase)
                .WithMany(phase => phase.ProjectTask)
                .HasForeignKey(projectTask => projectTask.PhaseId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Effort>(entity =>
        {
            entity.HasKey(effort => effort.Id);
            entity.Property(effort => effort.TaskName)
                .HasMaxLength(200)
                .IsRequired();
            entity.HasOne(effort => effort.ProjectTask)
                .WithMany(projectTask => projectTask.Efforts)
                .HasForeignKey(effort => effort.ProjectTaskId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<ProjectSubTask>(entity =>
        {
            entity.HasKey(subTask => subTask.Id);
            entity.Property(subTask => subTask.TaskName)
                .HasMaxLength(200)
                .IsRequired();
            entity.HasOne(subTask => subTask.ProjectTask)
                .WithMany(projectTask => projectTask.ProjectSubTasks)
                .HasForeignKey(subTask => subTask.ProjectTaskId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Assignment>(entity =>
        {
            entity.HasKey(assignment => new
            {
                assignment.ProjectTaskId,
                assignment.EmployeeId
            });
            entity.HasOne(assignment => assignment.ProjectTask)
                .WithMany(projectTask => projectTask.Assignments)
                .HasForeignKey(assignment => assignment.ProjectTaskId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(assignment => assignment.Employee)
                .WithMany(employee => employee.Assignments)
                .HasForeignKey(assignment => assignment.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
