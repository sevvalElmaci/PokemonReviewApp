using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace PokemonReviewApp.Dto
{
    public class PropertyDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

    }
    public class PropertyDtoUpdate
    {
        
        public string Name { get; set; }

    }
}




