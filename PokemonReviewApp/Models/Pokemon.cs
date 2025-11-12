namespace PokemonReviewApp.Models
{
    public class Pokemon : AuditEntityBase
    {
        //Models are just representative database tables (database table like an excel sheet)
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public ICollection<Review> Reviews { get; set; }
        public ICollection<PokemonCategory> PokemonCategories { get; set; }
        public ICollection<PokemonOwner> PokemonOwners { get; set; }
        public ICollection<PokeFood> PokeFoods { get; set; }
        
        public ICollection<PokeProperty> PokeProperties { get; set; }
        //
        //navigation property


    }
}
