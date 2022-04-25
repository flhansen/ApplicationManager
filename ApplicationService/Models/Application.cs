using System.ComponentModel.DataAnnotations.Schema;

namespace ApplicationService.Models;

[Table("application")]
public class Application
{
    [Column("id")]
    public int Id { get; set; }

    [Column("user_id")]
    public int UserId { get; set; }

    [Column("job_title")]
    public string JobTitle { get; set; } = string.Empty;

    [Column("work_type_id")]
    public int? WorkType { get; set; }

    [Column("company_name")]
    public string CompanyName { get; set; } = string.Empty;

    [Column("submission_date")]
    public DateTime? SubmissionDate { get; set; }

    [Column("status_id")]
    public int? Status { get; set; }

    [Column("wanted_salary")]
    public double? WantedSalary { get; set; }

    [Column("accepted_salary")]
    public double? AcceptedSalary { get; set; }

    [Column("start_date")]
    public DateTime? StartDate { get; set; }

    [Column("commentary")]
    public string? Commentary { get; set; }
}