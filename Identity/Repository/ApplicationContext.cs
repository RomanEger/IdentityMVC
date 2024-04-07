using Identity.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Identity.Repository;

public class ApplicationContext : DbContext
{
    public DbSet<User> Users { get; set; }

    public ApplicationContext()
    {
        
    }

    public ApplicationContext(DbContextOptions<ApplicationContext> options)
        :base(options)
    {
        
    }
}