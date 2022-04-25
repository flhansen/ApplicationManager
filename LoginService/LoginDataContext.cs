using Microsoft.EntityFrameworkCore;
using LoginService.Models;

namespace LoginService;

public class LoginDataContext : DbContext
{
    public LoginDataContext(DbContextOptions<LoginDataContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
}