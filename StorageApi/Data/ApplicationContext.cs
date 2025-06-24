using Microsoft.EntityFrameworkCore;
using StorageApi.Models;

namespace StorageApi.Data;


public class ApplicationContext : DbContext
{
    public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Product { get; set; } = default!;
}
