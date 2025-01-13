using BaseServiceContracts.Feature.PartnerZoneCommand.Create;
using BaseServiceContracts.Feature.PartnerZoneCommand.Remove;
using BaseServiceContracts.Feature.PartnerZoneCommand.Update;
using BaseServiceLibrary.DTO.ResponseDto;
using IdentityMoq;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace PartnerZoneService.Controllers;

[ApiController]
[Route("[controller]")]
public class IdentityController : ControllerBase
{
    [HttpPost("GiveAccessToAccount")]
    public async Task<IActionResult> Create(Guid targetAccountId, Guid targetUserId)
    {
        MoqIdentityService.GiveAccessToCreateZone(targetAccountId, targetUserId);
        return Ok("Permission Granted");
    }
}