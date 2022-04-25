using System.ComponentModel.DataAnnotations.Schema;

namespace ApplicationService.Models;

[Table("work_type")]
public class WorkType 
{
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    public string? Name { get; set; }
}