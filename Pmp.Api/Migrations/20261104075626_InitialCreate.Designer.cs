
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Pmp.Api.Data;

#nullable disable

namespace Pmp.Api.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20261104075626_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "10.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Pmp.Api.Models.Assignment", b =>
                {
                    b.Property<int>("ProjectTaskId")
                        .HasColumnType("int");

                    b.Property<int>("EmployeeId")
                        .HasColumnType("int");

                    b.HasKey("ProjectTaskId", "EmployeeId");

                    b.HasIndex("EmployeeId");

                    b.ToTable("Assignments");
                });

            modelBuilder.Entity("Pmp.Api.Models.ClientContact", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ContactName")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<int>("CustomerId")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(320)
                        .HasColumnType("nvarchar(320)");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.ToTable("ClientContacts");
                });

            modelBuilder.Entity("Pmp.Api.Models.Customer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTimeOffset>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetimeoffset")
                        .HasDefaultValueSql("SYSUTCDATETIME()");

                    b.Property<string>("CustomerName")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("Id");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("Pmp.Api.Models.Effort", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ProjectTaskId")
                        .HasColumnType("int");

                    b.Property<string>("TaskName")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("Id");

                    b.HasIndex("ProjectTaskId");

                    b.ToTable("Efforts");
                });

            modelBuilder.Entity("Pmp.Api.Models.Employee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("Id");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("Pmp.Api.Models.Phase", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("PhaseName")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<int>("ReleaseId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ReleaseId");

                    b.ToTable("Phases");
                });

            modelBuilder.Entity("Pmp.Api.Models.Project", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal?>("ActualHours")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetimeoffset")
                        .HasDefaultValueSql("SYSUTCDATETIME()");

                    b.Property<int>("CustomerId")
                        .HasColumnType("int");

                    b.Property<string>("Domain")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<DateTimeOffset>("EndDate")
                        .HasColumnType("datetimeoffset");

                    b.Property<decimal>("EstimatedHours")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTimeOffset>("IssueDate")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Methodology")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<int>("OffshoreManagerId")
                        .HasColumnType("int");

                    b.Property<int>("OnshoreManagerId")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset?>("RenewalDate")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTimeOffset?>("RevisedEndDate")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Sector")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<DateTimeOffset>("StartDate")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("Id");

                    b.HasIndex("Code")
                        .IsUnique();

                    b.HasIndex("CustomerId");

                    b.HasIndex("OffshoreManagerId");

                    b.HasIndex("OnshoreManagerId");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("Pmp.Api.Models.ProjectSubTask", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ProjectTaskId")
                        .HasColumnType("int");

                    b.Property<string>("TaskName")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("Id");

                    b.HasIndex("ProjectTaskId");

                    b.ToTable("ProjectSubTasks");
                });

            modelBuilder.Entity("Pmp.Api.Models.ProjectTask", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("PhaseId")
                        .HasColumnType("int");

                    b.Property<string>("TaskName")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("Id");

                    b.HasIndex("PhaseId");

                    b.ToTable("ProjectTasks");
                });

            modelBuilder.Entity("Pmp.Api.Models.ProjectWorkstream", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ProjectId")
                        .HasColumnType("int");

                    b.Property<string>("StreamName")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.ToTable("ProjectWorkstream");
                });

            modelBuilder.Entity("Pmp.Api.Models.Release", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ProjectWorkstreamId")
                        .HasColumnType("int");

                    b.Property<string>("ReleaseName")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("Id");

                    b.HasIndex("ProjectWorkstreamId");

                    b.ToTable("Releases");
                });

            modelBuilder.Entity("Pmp.Api.Models.Stakeholder", b =>
                {
                    b.Property<int>("ProjectId")
                        .HasColumnType("int");

                    b.Property<int>("EmployeeId")
                        .HasColumnType("int");

                    b.HasKey("ProjectId", "EmployeeId");

                    b.HasIndex("EmployeeId");

                    b.ToTable("Stakeholder");
                });

            modelBuilder.Entity("Pmp.Api.Models.Assignment", b =>
                {
                    b.HasOne("Pmp.Api.Models.Employee", "Employee")
                        .WithMany("Assignments")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Pmp.Api.Models.ProjectTask", "ProjectTask")
                        .WithMany("Assignments")
                        .HasForeignKey("ProjectTaskId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Employee");

                    b.Navigation("ProjectTask");
                });

            modelBuilder.Entity("Pmp.Api.Models.ClientContact", b =>
                {
                    b.HasOne("Pmp.Api.Models.Customer", "Customer")
                        .WithMany()
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("Pmp.Api.Models.Effort", b =>
                {
                    b.HasOne("Pmp.Api.Models.ProjectTask", "ProjectTask")
                        .WithMany("Efforts")
                        .HasForeignKey("ProjectTaskId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ProjectTask");
                });

            modelBuilder.Entity("Pmp.Api.Models.Phase", b =>
                {
                    b.HasOne("Pmp.Api.Models.Release", "Release")
                        .WithMany("Phases")
                        .HasForeignKey("ReleaseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Release");
                });

            modelBuilder.Entity("Pmp.Api.Models.Project", b =>
                {
                    b.HasOne("Pmp.Api.Models.Customer", "Customer")
                        .WithMany("Projects")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Pmp.Api.Models.Employee", "OffshoreManager")
                        .WithMany()
                        .HasForeignKey("OffshoreManagerId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Pmp.Api.Models.Employee", "OnshoreManager")
                        .WithMany()
                        .HasForeignKey("OnshoreManagerId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Customer");

                    b.Navigation("OffshoreManager");

                    b.Navigation("OnshoreManager");
                });

            modelBuilder.Entity("Pmp.Api.Models.ProjectSubTask", b =>
                {
                    b.HasOne("Pmp.Api.Models.ProjectTask", "ProjectTask")
                        .WithMany("ProjectSubTasks")
                        .HasForeignKey("ProjectTaskId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ProjectTask");
                });

            modelBuilder.Entity("Pmp.Api.Models.ProjectTask", b =>
                {
                    b.HasOne("Pmp.Api.Models.Phase", "Phase")
                        .WithMany("ProjectTask")
                        .HasForeignKey("PhaseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Phase");
                });

            modelBuilder.Entity("Pmp.Api.Models.ProjectWorkstream", b =>
                {
                    b.HasOne("Pmp.Api.Models.Project", "Project")
                        .WithMany("Workstreams")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Project");
                });

            modelBuilder.Entity("Pmp.Api.Models.Release", b =>
                {
                    b.HasOne("Pmp.Api.Models.ProjectWorkstream", "ProjectWorkstream")
                        .WithMany("Releases")
                        .HasForeignKey("ProjectWorkstreamId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ProjectWorkstream");
                });

            modelBuilder.Entity("Pmp.Api.Models.Stakeholder", b =>
                {
                    b.HasOne("Pmp.Api.Models.Employee", "Employee")
                        .WithMany()
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Pmp.Api.Models.Project", "Project")
                        .WithMany("Stakeholders")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Employee");

                    b.Navigation("Project");
                });

            modelBuilder.Entity("Pmp.Api.Models.Customer", b =>
                {
                    b.Navigation("Projects");
                });

            modelBuilder.Entity("Pmp.Api.Models.Employee", b =>
                {
                    b.Navigation("Assignments");
                });

            modelBuilder.Entity("Pmp.Api.Models.Phase", b =>
                {
                    b.Navigation("ProjectTask");
                });

            modelBuilder.Entity("Pmp.Api.Models.Project", b =>
                {
                    b.Navigation("Stakeholders");

                    b.Navigation("Workstreams");
                });

            modelBuilder.Entity("Pmp.Api.Models.ProjectTask", b =>
                {
                    b.Navigation("Assignments");

                    b.Navigation("Efforts");

                    b.Navigation("ProjectSubTasks");
                });

            modelBuilder.Entity("Pmp.Api.Models.ProjectWorkstream", b =>
                {
                    b.Navigation("Releases");
                });

            modelBuilder.Entity("Pmp.Api.Models.Release", b =>
                {
                    b.Navigation("Phases");
                });
#pragma warning restore 612, 618
        }
    }
}
