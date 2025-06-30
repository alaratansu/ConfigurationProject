using Configuration.Application.Common.Interface;
using Configuration.Application.Messaging;
using Configuration.Domain.Entities;
using Configuration.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Configuration.Application.ConfigurationItems.Commands;

public class UpdateConfigurationItemCommandHandler : IRequestHandler<UpdateConfigurationItemCommand, ConfigurationItem>
{
    private readonly ConfigurationDbContext _dbContext;
    private readonly IMessagePublisher _publisher;

    public UpdateConfigurationItemCommandHandler(ConfigurationDbContext dbContext,
        IMessagePublisher publisher)
    {
        _dbContext = dbContext;
        _publisher = publisher;
    }

    public async Task<ConfigurationItem> Handle(UpdateConfigurationItemCommand request, 
        CancellationToken cancellationToken)
    {
        var item = await _dbContext.ConfigurationItems
            .FirstOrDefaultAsync(x => x.Id == request.Item.Id, cancellationToken);

        if (item is null)
            throw new KeyNotFoundException("Item not found");

        item.Name = request.Item.Name;
        item.Type = request.Item.Type;
        item.Value = request.Item.Value;
        item.IsActive = request.Item.IsActive;
        item.ApplicationName = request.Item.ApplicationName;
        item.UpdatedAt = DateTime.UtcNow;
        
        await _dbContext.SaveChangesAsync(cancellationToken);

        _publisher.Publish(new ConfigurationChangedEvent
        {
            Item = item,
            ChangeType = "Update"
        });
        
        return item;
    }
}