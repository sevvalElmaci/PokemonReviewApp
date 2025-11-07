namespace PokemonReviewApp.Dto
{
    public class PokeFoodDto
    {
        public double Quantity { get; set; }

        public int PokemonId { get; set; }
        public int FoodId { get; set; }
        public string PokemonName { get; set; }
        public string FoodName { get; set; }
    }
}
