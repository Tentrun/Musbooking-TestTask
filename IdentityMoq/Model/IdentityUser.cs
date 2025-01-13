namespace IdentityMoq.Model;

/// <summary>
/// Условная identity модель юзера
/// </summary>
public class IdentityUser
{
    public Guid Id { get; set; }

    public IdentityRoles Role { get; set; } = IdentityRoles.Unverified;
    public List<Guid> AccountsIdToZoneCreate {get; set;} = new List<Guid>();
}