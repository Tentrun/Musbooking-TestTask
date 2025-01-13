using System.Diagnostics.Contracts;
using IdentityMoq.Model;

namespace IdentityMoq;

public static class IdentityUsers
{
    /// <summary>
    /// Moq данные identity users
    /// </summary>
    public static List<IdentityUser> IdentityUsersList = new List<IdentityUser>
    {
        new IdentityUser
        {
            Id = Guid.Parse("3cbe8efb-8636-4498-80fa-cf3d75882876"),
            Role = IdentityRoles.Verified
        },
        new IdentityUser
        {
            Id = Guid.Parse("e779fa70-9040-4a11-8729-a430b56a0c6f"),
        },
        new IdentityUser
        {
            Id = Guid.Parse("07007776-3ad8-4d8e-b91a-334895887648"),
            Role = IdentityRoles.Admin
        },
    };

}