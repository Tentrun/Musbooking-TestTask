using IdentityMoq.Model;

namespace IdentityMoq;

public static class MoqIdentityService
{
    /// <summary>
    /// Кешируем дефолт юзеров
    /// </summary>
    private static readonly List<IdentityUser> CachedIdentityUsers = IdentityUsers.IdentityUsersList;
    
    /// <summary>
    /// Блокировщик потока
    /// </summary>
    private static readonly object Locker = new object();

    /// <summary>
    /// Сброс юзеров до стандартных настроек
    /// </summary>
    public static void ResetIdentityUsers()
    {
        IdentityUsers.IdentityUsersList = CachedIdentityUsers;
    }

    public static void GiveAccessToCreateZone(Guid accountId, Guid targetAccountId)
    {
        lock (Locker)
        {
            IdentityUser identityUser = IdentityUsers.IdentityUsersList.First(x => x.Id == accountId);
            IdentityUsers.IdentityUsersList.Remove(identityUser);

            identityUser.AccountsIdToZoneCreate.Add(targetAccountId);
            if (identityUser.Role == IdentityRoles.Unverified)
            {
                identityUser.Role = IdentityRoles.Verified;
            }
            
            IdentityUsers.IdentityUsersList.Add(identityUser);
        }
    }

    public static bool UserCanEditAccount(Guid accountId, Guid targetAccountId)
    {
        IdentityUser user = IdentityUsers.IdentityUsersList.First(x => x.Id == accountId);
        
        return user.Role != IdentityRoles.Unverified && user.AccountsIdToZoneCreate.Contains(targetAccountId) || user.Role == IdentityRoles.Admin;
    }
    
    public static void RemoveAccessToCreateZone(Guid accountId, Guid targetAccountId)
    {
        lock (Locker)
        {
            IdentityUser identityUser = IdentityUsers.IdentityUsersList.First(x => x.Id == accountId && x.AccountsIdToZoneCreate.Contains(targetAccountId));
            IdentityUsers.IdentityUsersList.Remove(identityUser);
            identityUser.AccountsIdToZoneCreate.Remove(targetAccountId);

            if (identityUser.AccountsIdToZoneCreate.Count == 0)
            {
                identityUser.Role = IdentityRoles.Unverified;
            }
        
            IdentityUsers.IdentityUsersList.Add(identityUser);
        }

    }
}