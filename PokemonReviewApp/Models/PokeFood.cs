using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PokemonReviewApp.Models
{
    public class PokeFood : AuditEntityBase
    {
        public int PokemonId { get; set; }
        public int FoodId { get; set; }
        public Pokemon Pokemon { get; set; }
        public Food Food { get; set; }
        public double Quantity { get; set; }   // Foreign Key
    }
}
