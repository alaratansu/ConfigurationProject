using Configuration.Domain.Entities;

namespace Configuration.Application.Messaging;

public class ConfigurationChangedEvent
{
    public string ChangeType { get; set; }
    public ConfigurationItem Item { get; set; }
}