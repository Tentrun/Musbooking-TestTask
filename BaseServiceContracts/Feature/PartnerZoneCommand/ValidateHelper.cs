using BaseServiceContracts.Feature.PartnerZoneCommand.Create;
using BaseServiceContracts.Feature.PartnerZoneCommand.Remove;
using BaseServiceContracts.Feature.PartnerZoneCommand.Update;
using BaseServiceContracts.Interfaces.Repositories.Implementations;
using BaseServiceLibrary.Entity.Base;
using IdentityMoq;

namespace BaseServiceContracts.Feature.PartnerZoneCommand;

public static class ValidateHelper
{
    public static bool ValidateRequest(PartnerZoneCreateCommand request)
    {
        //Проверяем наличие targed accounts
        bool accountsExist = IdentityUsers.IdentityUsersList.FirstOrDefault(x => x.Id == request.CreatorUserId) != null
                             && IdentityUsers.IdentityUsersList.FirstOrDefault(x => x.Id == request.AccountId) != null; 
            
        if (!accountsExist)
        {
            throw new ArgumentException("Account not found");
        }
            
        bool isElevated = MoqIdentityService.UserCanEditAccount(request.CreatorUserId, request.AccountId);
        if (!isElevated)
        {
            throw new UnauthorizedAccessException($"Account {request.CreatorUserId} does not have access to edit {request.AccountId}");
        }
        
        return true;
    }
    
    public static async Task ValidateRequest(PartnerZoneRemoveCommand request)
    {
        //Проверяем наличие targed accounts
        bool accountExist = IdentityUsers.IdentityUsersList.FirstOrDefault(x => x.Id == request.CreatorUserId) != null;
        if (!accountExist)
        {
            throw new ArgumentException($"Account not found");
        }
                
        bool isElevated = IdentityUsers.IdentityUsersList.First(x => x.Id == request.CreatorUserId).Role == IdentityRoles.Admin;
        if (!isElevated)
        {
            throw new UnauthorizedAccessException($"Account {request.CreatorUserId} does not have access");
        }
    }
    
    public static async Task ValidateRequest(PartnerZoneUpdateCommand request, PartnerZone existingPartnerZone)
    {
        //Проверяем наличие targed accounts
        bool accountsExist = IdentityUsers.IdentityUsersList.FirstOrDefault(x => x.Id == request.CreatorUserId) != null
            && request.AccountId == null || IdentityUsers.IdentityUsersList.FirstOrDefault(x => x.Id == request.CreatorUserId) != null;

        if (!accountsExist)
        {
            throw new ArgumentException("Account not found");
        }
            
        bool isElevated = MoqIdentityService.UserCanEditAccount(request.CreatorUserId, existingPartnerZone.AccountId);
        if (!isElevated)
        {
            throw new UnauthorizedAccessException($"Account {request.CreatorUserId} does not have access to edit {existingPartnerZone.AccountId}");
        }
    }
}