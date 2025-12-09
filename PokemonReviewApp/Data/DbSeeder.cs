using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Models;

public static class DbSeeder
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        // ============================
        // 1) ANA DOMAINLER
        // ============================
        string[] mainDomains = new[]
        {
            "Category",
            "Country",
            "Food",
            "Owner",
            "Pokemon",
            "Property",
            "Review",
            "Reviewer",
            "User"       // Manager CRUD yapacak
        };

        // ============================
        // 2) ADMIN DOMAINLERİ
        // (Sadece admin görebilir)
        // ============================
        string[] adminOnlyDomains = new[]
        {
            "UserLog",
            "PokemonLog",
            "Permission",
            "RolePermission",
            "Role"
        };

        // ============================
        // 3) ACTION SET
        // ============================
        string[] actions = new[]
        {
            "List",
            "Add",
            "Update",
            "Delete",
            "Restore",
            "ListDeleted"
        };

        // ============================
        // 4) PERMISSION GENERATION
        // ============================
        var permissions = new List<Permission>();
        int pid = 1;

        // Main domain permissions
        foreach (var domain in mainDomains)
        {
            foreach (var action in actions)
            {
                permissions.Add(new Permission
                {
                    Id = pid++,
                    PermissionName = $"{domain}.{action}"
                });
            }
        }

        // Admin-only domain permissions (admin full access)
        foreach (var domain in adminOnlyDomains)
        {
            foreach (var action in actions)
            {
                permissions.Add(new Permission
                {
                    Id = pid++,
                    PermissionName = $"{domain}.{action}"
                });
            }
        }

        modelBuilder.Entity<Permission>().HasData(permissions);

        // ============================
        // 5) ROLES
        // ============================
        modelBuilder.Entity<Role>().HasData(
            new Role { Id = 1, RoleName = "Admin" },
            new Role { Id = 2, RoleName = "Manager" },
            new Role { Id = 3, RoleName = "User" }
        );

        // ============================
        // 6) ROLE -> PERMISSION MAPPING
        // ============================
        var rolePermissions = new List<RolePermission>();

        // ------------------------------------------
        // ADMIN -> FULL ACCESS
        // ------------------------------------------
        foreach (var perm in permissions)
        {
            rolePermissions.Add(new RolePermission
            {
                RoleId = 1,
                PermissionId = perm.Id
            });
        }

        // ------------------------------------------
        // MANAGER -> CRUD (DELETE/RESTORE/LISTDELETED YOK)
        // User için: CRUD 
        // ------------------------------------------
        // MANAGER: only List / Add / Update / Delete (NO Restore, NO ListDeleted)
        foreach (var perm in permissions)
        {
            bool isAdminOnlyDomain = adminOnlyDomains
                .Any(d => perm.PermissionName.StartsWith(d + "."));

            if (isAdminOnlyDomain)
                continue;

            bool isAllowed =
                perm.PermissionName.EndsWith(".List") ||
                perm.PermissionName.EndsWith(".Add") ||
                perm.PermissionName.EndsWith(".Update") ||
                perm.PermissionName.EndsWith(".Delete");

            if (isAllowed)
            {
                rolePermissions.Add(new RolePermission
                {
                    RoleId = 2,
                    PermissionId = perm.Id
                });
            }
        }


        // ------------------------------------------
        // USER -> Only List (+ Review.Add)
        // ------------------------------------------
        foreach (var perm in permissions)
        {
            bool allowed =
                perm.PermissionName.EndsWith(".List") ||
                perm.PermissionName == "Review.Add";

            // Admin-only domain? User access asla yok.
            if (adminOnlyDomains.Any(ad => perm.PermissionName.StartsWith(ad + ".")))
                allowed = false;

            // User "User" domainini göremez
            if (perm.PermissionName.StartsWith("User."))
                allowed = false;

            if (allowed)
            {
                rolePermissions.Add(new RolePermission
                {
                    RoleId = 3,
                    PermissionId = perm.Id
                });
            }
        }

        modelBuilder.Entity<RolePermission>().HasData(rolePermissions);
    }
}
