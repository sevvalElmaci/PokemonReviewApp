namespace PokemonReviewApp.Models
{
    public class PokeFood
    {
        public int PokemonId { get; set; }
        public int FoodId { get; set; }
        public Pokemon Pokemon { get; set; }
        public Food Food { get; set; }
    }
}
