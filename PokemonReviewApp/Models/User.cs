namespace PokemonReviewApp.Models
{
    public class User : AuditEntityBase
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public ICollection<UserRole>? UserRoles { get; set; }
    }
}
