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
        bool CreateCategory(Category category, int userId);
        bool UpdateCategory(Category category, int userId);

        //bool DeleteCategory(Category category);
       bool SoftDeleteCategory(int categoryId, int userId);
        Category GetCategoryIncludingDeleted(int id);
        void RestoreCategory(Category category);

        ICollection<Category> GetCategoriesIncludingDeleted();

        ICollection<Category> GetDeletedCategories();  


        bool Save();    

    }

}
