namespace PokemonReviewApp.Models
{
    public class Category : AuditEntityBase
    {
        public int Id { get; set; } 
        public string Name { get; set; }

        public ICollection<PokemonCategory> PokemonCategories { get; set; }
        public User? CreatedUser { get; set; }


    }
}
