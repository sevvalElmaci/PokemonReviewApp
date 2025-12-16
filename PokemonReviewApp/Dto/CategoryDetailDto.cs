namespace PokemonReviewApp.Dto
{
    public class CategoryDetailDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string CreatedBy { get; set; }

        public List<string> Pokemons { get; set; }
    }

}
