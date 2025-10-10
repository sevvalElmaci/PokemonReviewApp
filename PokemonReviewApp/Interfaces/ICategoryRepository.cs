using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{ 
    public interface ICategoryRepository
    {
        ICollection<Category> GetCategories();
        Category GetCategory(int id); //ID is the primary key thats why we just write id but not name
        // let say fire category id is 1 but client can write eith different forms:
        //FIRE, fire, Fire
        ICollection<Pokemon> GetPokemonByCategory(int categoryId);
        bool CategoryExist(int id); 
    
    }

}