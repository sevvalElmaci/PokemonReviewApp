namespace PokemonReviewApp.Dto
{
    public class CountryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class CountryDtoCreate
    {
        public string Name { get; set; }
    }
}
