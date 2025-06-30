using Configuration.Domain.Entities;
using Configuration.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Configuration.Application.ConfigurationItems.Queries;

public class GetConfigurationItemsQueryHandler : IRequestHandler<GetConfigurationItemsQuery, List<ConfigurationItem>>
{
    private readonly ConfigurationDbContext dbContext;

    public GetConfigurationItemsQueryHandler(ConfigurationDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<List<ConfigurationItem>> Handle(GetConfigurationItemsQuery request, CancellationToken cancellationToken)
    {
        return await dbContext.ConfigurationItems
            .Where(x => string.IsNullOrEmpty(request.SiteName) || x.Name.Equals(request.SiteName))
            .ToListAsync(cancellationToken);
    }
}