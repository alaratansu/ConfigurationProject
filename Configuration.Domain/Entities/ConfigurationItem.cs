namespace Configuration.Domain.Entities;

public class ConfigurationItem
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Type { get; set; } = default!; 
    public string Value { get; set; } = default!;
    public bool IsActive { get; set; }
    public string ApplicationName { get; set; } = default!;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}