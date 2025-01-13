using System.Text.Json;
using BaseServiceContracts.Feature.PartnerZoneCommand.Create;
using BaseServiceContracts.Feature.PartnerZoneCommand.Get;
using BaseServiceContracts.Feature.PartnerZoneCommand.Remove;
using BaseServiceContracts.Feature.PartnerZoneCommand.Update;
using BaseServiceLibrary.DTO.PartnerZoneDto.Get;
using BaseServiceLibrary.DTO.ResponseDto;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace PartnerZoneService.Controllers;

[ApiController]
[Route("[controller]")]
public class PartnerZoneController : ControllerBase
{
    private readonly IMediator _mediator;
    
    private readonly ILogger<PartnerZoneController> _logger;

    public PartnerZoneController(ILogger<PartnerZoneController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        try
        {
            PartnerZoneGetDto result = await _mediator.Send(new PartnerZoneGetCommand(), cancellationToken);
            return Ok(new ResponseDto(JsonSerializer.Serialize(result) ,200));
        }
        catch (Exception e)
        {
            return Ok(new ResponseDto(e.Message, 500));
        }
    }
    
    [HttpPost("Update")]
    public async Task<IActionResult> Update(PartnerZoneUpdateCommand partnerZone, CancellationToken token)
    {
        try
        {
            await _mediator.Send(partnerZone, token);
            return Ok(new ResponseDto("Success", 200));
        }
        catch (Exception e)
        {
            return Ok(new ResponseDto(e.Message, 500));
        }
    }
    
    [HttpPost("Remove")]
    public async Task<IActionResult> Remove(PartnerZoneRemoveCommand partnerZone, CancellationToken token)
    {
        try
        {
            await _mediator.Send(partnerZone, token);
            return Ok(new ResponseDto("Success", 200));
        }
        catch (Exception e)
        {
            return Ok(new ResponseDto(e.Message, 500));
        }
    }
    
    [HttpPost("Create")]
    public async Task<IActionResult> Create(PartnerZoneCreateCommand partnerZone, CancellationToken token)
    {
        try
        {
            await _mediator.Send(partnerZone, token);
            return Ok(new ResponseDto("Success", 200));
        }
        catch (Exception e)
        {
            return Ok(new ResponseDto(e.Message, 500));
        }
    }
}