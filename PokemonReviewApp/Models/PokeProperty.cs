namespace PokemonReviewApp.Models
{
    public class PokeProperty
    {
        public int PokemonId { get; set; }
        public int PropertyId { get; set; }
        public Pokemon Pokemon { get; set; }
        public Property Property { get; set; }
    }
}
