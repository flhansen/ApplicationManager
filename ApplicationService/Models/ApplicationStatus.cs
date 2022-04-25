using System.ComponentModel.DataAnnotations.Schema;

namespace ApplicationService.Models;

[Table("application_status")]
public class ApplicationStatus
{
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    public string? Name { get; set; }
}