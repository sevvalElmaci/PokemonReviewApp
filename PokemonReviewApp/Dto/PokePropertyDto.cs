namespace PokemonReviewApp.Dto
{
    public class PokePropertyDto
    {
        public int PokemonId { get; set; }
        public string PokemonName { get; set; }    
        public int PropertyId { get; set; }
        public string PropertyName { get; set; }
    }
    public class PokePropertyDtoCreate
    {
        public int PokemonId { get; set; }
        public int PropertyId { get; set; }
    }
    public class PokePropertyDtoUpdate
    {
        public int PokemonId { get; set; }
        public string PokemonName { get; set; }
        public int PropertyId { get; set; }
        public string PropertyName { get; set; }

        public int ChangedPokemonId { get; set; }
        public int ChangedPropertyId { get; set; }

    }

    public class PokePropertyDtoUpdateUpdate
    {
        public int PokemonId { get; set; }
        public int PropertyId { get; set; }
        public int ChangedPokemonId { get; set; }
        public int ChangedPropertyId { get; set; }



    }
}
