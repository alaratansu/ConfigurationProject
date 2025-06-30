using FluentValidation;

namespace Configuration.Application.ConfigurationItems.Commands;

public class CreateConfigurationItemCommandValidator : AbstractValidator<CreateConfigurationItemCommand>
{
    public CreateConfigurationItemCommandValidator()
    {
        RuleFor(x => x.Item.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Item.Value).NotEmpty();
        RuleFor(x => x.Item.Type).NotEmpty();
        RuleFor(x => x.Item.ApplicationName).NotEmpty();
    }
}