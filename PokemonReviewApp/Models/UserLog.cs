namespace PokemonReviewApp.Models
{
    public class UserLog : AuditEntityBase
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        public string UserName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}
