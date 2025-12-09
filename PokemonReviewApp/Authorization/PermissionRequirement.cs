using Microsoft.AspNetCore.Authorization;

namespace PokemonReviewApp.Authıorization
{
    public class PermissionRequirement : IAuthorizationRequirement
    {
        public string PermissionName { get; }

        public PermissionRequirement(string permissionName)
        {
            PermissionName = permissionName;
        }
    }


}
