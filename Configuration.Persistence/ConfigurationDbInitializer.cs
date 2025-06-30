using Configuration.Domain.Entities;

namespace Configuration.Persistence;

public static class ConfigurationDbInitializer
{
    public static void Initialize(ConfigurationDbContext context)
    {
        if (!context.ConfigurationItems.Any())
        {
            context.ConfigurationItems.AddRange(new[]
            {
                new ConfigurationItem
                {
                    Name = "SiteName",
                    Type = "String",
                    Value = "Boyner.com.tr",
                    IsActive = true,
                    ApplicationName = "SERVICE-A",
                    UpdatedAt = DateTime.UtcNow
                },
                new ConfigurationItem
                {
                    Name = "IsBasketEnabled",
                    Type = "Boolean",
                    Value = "1",
                    IsActive = true,
                    ApplicationName = "SERVICE-B",
                    UpdatedAt = DateTime.UtcNow
                },
                new ConfigurationItem
                {
                    Name = "MaxItemCount",
                    Type = "Int",
                    Value = "50",
                    IsActive = false,
                    ApplicationName = "SERVICE-A",
                    UpdatedAt = DateTime.UtcNow
                }
            });

            context.SaveChanges();
        }
    }
}