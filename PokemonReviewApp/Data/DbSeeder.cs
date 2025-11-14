using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Models;
using System.Data;

namespace PokemonReviewApp.Data
{
    public static class DbSeeder
    {
        public static void Seed(ModelBuilder modelbuilder)
        {

            var permissionNames = new[]
            {
                "ListCategory", "AddCategory", "UpdateCategory", "DeleteCategory",
                "ListCountry", "AddCountry", "UpdateCountry", "DeleteCountry",
                "ListFood", "AddFood", "UpdateFood", "DeleteFood",
                "ListOwner", "AddOwner", "UpdateOwner", "DeleteOwner",
                "ListPokemon", "AddPokemon", "UpdatePokemon", "DeletePokemon",
                "ListProperty", "AddProperty", "UpdateProperty", "DeleteProperty",
                "ListReview", "AddReview", "UpdateReview", "DeleteReview",
                "ListReviewer", "AddReviewer", "UpdateReviewer", "DeleteReviewer",
            };
            var permissions = new List<Permission>();
            int pid = 1;

            foreach (var name in permissionNames)

            {
                permissions.Add(new Permission
                {
                    Id = pid++,
                    PermissionName = name,

                });
            }

            modelbuilder.Entity<Permission>().HasData(permissions);

            modelbuilder.Entity<Role>().HasData(

                new Role { Id = 1, RoleName = "Admin" },
                new Role { Id = 2, RoleName = "Manager" },
                new Role { Id = 3, RoleName = "User"}
                );

            var rolePermissions = new List<RolePermission>();


            for (int i = 1; i <= permissions.Count; i++)
            {
                rolePermissions.Add(new RolePermission
                {
                    RoleId = 1,
                    PermissionId = i,
                });
            }

            var managerAllowed = permissions
                .Where(p =>
                !p.PermissionName.Contains("DeleteCategory") &&
                !p.PermissionName.Contains("DeleteCountry") &&
                !p.PermissionName.Contains("DeleteFood") &&
                !p.PermissionName.Contains("DeleteOwner") &&
                !p.PermissionName.Contains("DeletePokemon") &&
                !p.PermissionName.Contains("DeleteProperty") &&
                !p.PermissionName.Contains("DeleteReview") &&
                !p.PermissionName.Contains("DeleteReviewer")
                );

            foreach (var perm in managerAllowed)
            {
                rolePermissions.Add(new RolePermission
                {
                    RoleId = 2,
                    PermissionId = perm.Id

                });
            }

            var UserAllowed = permissions
                .Where(p =>
                p.PermissionName.StartsWith("List") ||
                p.PermissionName == "AddReview")
                .Select(p => p.Id)
            .ToList();
            foreach (var userPermId in UserAllowed)
            {
                rolePermissions.Add(new RolePermission
                {
                    RoleId = 3,
                    PermissionId = userPermId

                });
            }

            modelbuilder.Entity<RolePermission>().HasData(rolePermissions);

        }
    }
}
