namespace PokemonReviewApp.Models
{
    public class Permission : AuditEntityBase
    {
        public int Id { get; set; }
        public string PermissionName { get; set; }
        public string? Description { get; set; }
        public ICollection<RolePermission>? RolePermissions { get; set; }
    }
}
