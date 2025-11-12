namespace PokemonReviewApp.Models
{
    public class Permission : AuditEntityBase
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty; // örnek: PokemonListele, PokemonEkle
        public ICollection<RolePermission>? RolePermissions { get; set; }
    }
}
