using System.ComponentModel.DataAnnotations;
using Pmp.Api.Models;

namespace Pmp.Api.Contracts;

public sealed class CreateProjectRequest
{
    [Required]
    public string Code{get;set;}=string.Empty;
    [Required]
    public string Name{get;set;}=string.Empty;

    [Range(1,int.MaxValue)]
    public int CustomerId{get;set;}

    [Range(1,int.MaxValue)]
    public int OnshoreManagerId{get;set;}

    [Range(1,int.MaxValue)]
    public int OffshoreManagerId{get;set;}
    public DateTimeOffset StartDate {get;set;}
    public DateTimeOffset EndDate{get;set;}
    public DateTimeOffset? RevisedEndDate {get;set;}
    public DateTimeOffset? RenewalDate {get;set;}
    public DateTimeOffset Issuedate {get;set;}

    [Range(0,double.MaxValue)]
    public decimal EstimatedHours{get;set;}
    public decimal? ActualHours{get;set;}
    public Domain Domain {get;set;}
    public ProjectType Type {get;set;}
    public Sector Sector {get;set;}
    public Methodology Methodology {get;set;}
    public ProjectStatus Status { get; set; }

}
    
