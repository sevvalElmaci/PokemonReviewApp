using System.ComponentModel.DataAnnotations.Schema;

namespace PokemonReviewApp.Models
{
    public class User : AuditEntityBase
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public int RoleId { get; set; }

        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }

        [ForeignKey("RoleId")]
       
        public Role Role { get; set; }
    }
}
