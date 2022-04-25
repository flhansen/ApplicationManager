using System.ComponentModel.DataAnnotations.Schema;

namespace LoginService.Models;

[Table("account")]
public class User
{
    [Column("id")]
    public int Id { get; set; }

    [Column("username")]
    public string? UserName { get; set; }

    [Column("password")]
    public string? Password { get; set; }

    [Column("email")]
    public string? Email { get; set; }

    [Column("creation_date")]
    public DateTime? CreationDate { get; set; }
}