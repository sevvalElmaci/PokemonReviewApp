namespace PokemonReviewApp.Models
{
    public class PokemonLog : AuditEntityBase
    {
        public int Id { get; set; }

        public int PokemonId {get; set;}
        public string Name { get; set; }
        public int OwnerId { get; set; }

    }
}
