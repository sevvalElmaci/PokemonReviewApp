namespace PokemonReviewApp.Dto
{
    public class PokeFoodDto
    {
        public int Quantity { get; set; }

        public int PokemonId { get; set; }
        public int FoodId { get; set; }
        public string PokemonName { get; set; }
        public string FoodName { get; set; }
    }

    public class PokeFoodDtoCreate
    {
        public int Quantity { get; set; }

        public int PokemonId { get; set; }
        public int FoodId { get; set; }
    }
    public class PokeFoodUpdateDto
    {
        public int Quantity { get; set; }

        public int ChangedPokemonId { get; set; }
        public int ChangedFoodId { get; set; }
    }

}
