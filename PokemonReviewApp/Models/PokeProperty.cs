using System.ComponentModel.DataAnnotations.Schema;

namespace PokemonReviewApp.Models
{
    public class PokeProperty
    {
        [ForeignKey(nameof(Pokemon))]
        public int PokemonId { get; set; }

        [ForeignKey(nameof(Property))]
        public int PropertyId { get; set; }
        public Pokemon Pokemon { get; set; }
        public Property Property { get; set; }
        //buradan anlıyoruz ki pokemon ve  property arasında many to many ilişki var
        //çünkü ara tablo var
        //bir pokemona birden fazla property eklenebilir
         //bir property birden fazla pokemona eklenebilir//

    
    }
}
