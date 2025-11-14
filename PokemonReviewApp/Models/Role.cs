namespace PokemonReviewApp.Models
{
    public class Role 
    {
        public int Id { get; set; }
        public string RoleName { get; set; }
        public ICollection<User> Users { get; set; } // 1 role → many users
        public ICollection<RolePermission> RolePermissions { get; set; } 


    }
}
