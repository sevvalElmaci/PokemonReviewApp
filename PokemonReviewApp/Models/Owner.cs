namespace PokemonReviewApp.Models
{
    public class Owner
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gym { get; set; }
        public Country Country { get; set; } //its not a list of object. associated with only one country

        public ICollection<PokemonOwner> PokemonOwners { get; set; }

    }
}