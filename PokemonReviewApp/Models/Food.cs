namespace PokemonReviewApp.Models
{
    public class Food
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<PokeFood> PokeFoods { get; set; }
    }
}
