using Configuration.Domain.Entities;
using MediatR;

namespace Configuration.Application.ConfigurationItems.Commands;

public class CreateConfigurationItemCommand : IRequest<ConfigurationItem>
{
    public ConfigurationItem Item { get; set; } = default!;
}