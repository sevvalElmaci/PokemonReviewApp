namespace PokemonReviewApp.Models
{
    public class Property
    {
        public  int Id { get; set; }
        public string Name { get; set; }

        public ICollection<PokeProperty> PokeProperties { get; set; }
        //her property'nin birden fazla pokemona sahip olabilecegini belirtiyoruz
    }
}
