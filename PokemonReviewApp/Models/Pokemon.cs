using System.Text.Json.Serialization;

namespace PokemonReviewApp.Models
{
    public class Pokemon : AuditEntityBase
    {
        //Models are just representative database tables (database table like an excel sheet)
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public User CreatedUser { get; set; }


        [JsonIgnore]
        public ICollection<Review> Reviews { get; set; }

        [JsonIgnore]
        public ICollection<PokemonCategory> PokemonCategories { get; set; }

        [JsonIgnore]
        public ICollection<PokemonOwner> PokemonOwners { get; set; }


        [JsonIgnore]
        public ICollection<PokeFood> PokeFoods { get; set; }

        [JsonIgnore]
        public ICollection<PokeProperty> PokeProperties { get; set; }


        //
        //navigation property


    }
}
