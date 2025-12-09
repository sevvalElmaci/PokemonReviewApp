using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PokemonReviewApp.Models
{
    public class PokeFood 
    {
        public int PokemonId { get; set; }
        public int FoodId { get; set; }
        public Pokemon Pokemon { get; set; }
        public Food Food { get; set; }
        public double Quantity { get; set; }   // Foreign Key



        public int CreatedUserId { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public int? UpdatedUserId { get; set; }  
        public DateTime? UpdatedDateTime { get; set; }

        public bool IsDeleted { get; set; }
        public int? DeletedUserId { get; set; }
        public DateTime? DeletedDateTime { get; set; }
    }
    //Join entity olduğu icin Audit entity base inherite edilmiyor.
    //SoftDelete gercek enttiylere yapılır. Pokemon, Food, Permission gibi. 
    //Soft delete yapınca ilişki bozulmuş oluyor ama veride kalıyor → sistem karmakarışık olur.
}
