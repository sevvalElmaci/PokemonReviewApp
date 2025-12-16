using PokemonReviewApp.Dto;
using PokemonReviewApp.Dto.PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{ 
    public interface ICategoryRepository
    {
        ICollection<Category> GetCategories();
      //ID is the primary key thats why we just write id but not name
                                      // let say fire category id is 1 but client can write eith different forms:
                                      //FIRE, fire, Fire
        CategoryDetailDto GetCategoryDetail(int id);
        ICollection<CategoryNewDetailDto> GetAllCategoriesNewDetail();

        Category GetCategoryEntity(int id);
        Category GetCategoryIncludingDeleted(int id);
        CategoryNewDetailDto GetCategoryNewDetail(int categoryId);

        ICollection<Pokemon> GetPokemonByCategory(int categoryId);
        bool CategoryExist(int id); 
        bool CreateCategory(Category category, int userId);
        bool UpdateCategory(Category category, int userId);

        //bool DeleteCategory(Category category);
       bool SoftDeleteCategory(int categoryId, int userId);
        void RestoreCategory(Category category);

        ICollection<Category> GetCategoriesIncludingDeleted();

        ICollection<Category> GetDeletedCategories();  


        bool Save();    

    }

}
