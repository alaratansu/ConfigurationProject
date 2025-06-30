using Configuration.Domain.Entities;
using Configuration.Persistence;
using MediatR;

namespace Configuration.Application.ConfigurationItems.Commands;

public class CreateConfigurationItemCommandHandler : IRequestHandler<CreateConfigurationItemCommand, ConfigurationItem>
{
    private readonly ConfigurationDbContext dbContext;

    public CreateConfigurationItemCommandHandler(ConfigurationDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<ConfigurationItem> Handle(CreateConfigurationItemCommand request, CancellationToken cancellationToken)
    {
        request.Item.UpdatedAt = DateTime.UtcNow;
        dbContext.ConfigurationItems.Add(request.Item);
        await dbContext.SaveChangesAsync(cancellationToken);
        return request.Item;
    }
}