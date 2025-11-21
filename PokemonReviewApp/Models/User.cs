using System.ComponentModel.DataAnnotations.Schema;

namespace PokemonReviewApp.Models
{
    public class User : AuditEntityBase
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }

        [ForeignKey("RoleId")]
       
        public Role Role { get; set; }
    }
}
