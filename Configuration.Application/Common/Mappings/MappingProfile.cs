using AutoMapper;
using Configuration.Application.ConfigurationItems.Commands;
using Configuration.Domain.DTO;
using Configuration.Domain.Entities;

namespace Configuration.Application.Common.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CreateConfigurationItemDto, CreateConfigurationItemCommand>();
        CreateMap<ConfigurationItem, ConfigurationItemResponseDto>();
        CreateMap<UpdateConfigurationItemDto, UpdateConfigurationItemCommand>()
            .ForMember(dest => dest.Item.Id, opt => opt.Ignore()); 
    }
}