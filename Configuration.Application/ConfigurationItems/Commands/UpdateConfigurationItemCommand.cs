using Configuration.Domain.Entities;
using MediatR;

namespace Configuration.Application.ConfigurationItems.Commands;

public class UpdateConfigurationItemCommand : IRequest<ConfigurationItem>
{
    public ConfigurationItem Item { get; set; } = null!;
}