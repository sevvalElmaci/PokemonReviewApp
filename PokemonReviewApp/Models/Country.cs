namespace PokemonReviewApp.Models
{
    public class Country : AuditEntityBase
    {
        public int Id { get; set; } 
        public string Name { get; set; }

        public ICollection<Owner> Owners { get; set; }

    }
}
