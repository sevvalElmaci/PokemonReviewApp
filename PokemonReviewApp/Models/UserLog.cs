namespace PokemonReviewApp.Models
{
    public class UserLog : AuditEntityBase
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
     
    }
}
