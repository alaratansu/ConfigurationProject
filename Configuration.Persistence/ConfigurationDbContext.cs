using Configuration.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Configuration.Persistence;

public class ConfigurationDbContext : DbContext
{
    public ConfigurationDbContext(DbContextOptions<ConfigurationDbContext> options) 
        : base(options) { }

    public DbSet<ConfigurationItem> ConfigurationItems => Set<ConfigurationItem>();
}