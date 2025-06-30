using AutoMapper;
using Configuration.Application.ConfigurationItems.Commands;
using Configuration.Application.ConfigurationItems.Queries;
using Configuration.Domain.DTO;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Configuration.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ConfigurationController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public ConfigurationController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] string siteName = "")
    {
        var query = new GetConfigurationItemsQuery{ SiteName = siteName };
        var result = await _mediator.Send(query);
        return Ok(_mapper.Map<List<ConfigurationItemResponseDto>>(result));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateConfigurationItemDto dto)
    {
        var command = _mapper.Map<UpdateConfigurationItemCommand>(dto);
        var result = await _mediator.Send(command);
        return Ok(result);
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateConfigurationItemDto dto)
    {
        var command = _mapper.Map<UpdateConfigurationItemCommand>(dto);
        command.Item.Id = id; 

        var result = await _mediator.Send(command);
        return Ok(result);
    }
}