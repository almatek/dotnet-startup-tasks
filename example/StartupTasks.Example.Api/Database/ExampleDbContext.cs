using Microsoft.EntityFrameworkCore;

public class ExampleDbContext : DbContext
{
    public ExampleDbContext(DbContextOptions<ExampleDbContext> options)
        : base(options)
    {
    }

    public DbSet<ExampleEntity> ExampleEntities { get; set; }
}
