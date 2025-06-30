using Configuration.Domain.Entities;
using MediatR;

namespace Configuration.Application.ConfigurationItems.Queries;

public class GetConfigurationItemsQuery : IRequest<List<ConfigurationItem>>
{
    public string? SiteName { get; set; }
}