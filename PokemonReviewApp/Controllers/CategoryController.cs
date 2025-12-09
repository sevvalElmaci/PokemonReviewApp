using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;
using PokemonReviewApp.Helpers;

namespace PokemonReviewApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        // =============================
        // LIST
        // =============================
        [Authorize(Policy = "Category.List")]
        [HttpGet]
        public IActionResult GetCategories()
        {
            var categories = _mapper.Map<List<CategoryDto>>(
                _categoryRepository.GetCategories());

            return Ok(categories);
        }

        [Authorize(Policy = "Category.List")]
        [HttpGet("{categoryId}")]
        public IActionResult GetCategory(int categoryId)
        {
            if (!_categoryRepository.CategoryExist(categoryId))
                return NotFound();

            var category = _mapper.Map<CategoryDto>(
                _categoryRepository.GetCategory(categoryId));

            return Ok(category);
        }

        [Authorize(Policy = "Category.List")]
        [HttpGet("pokemon/{categoryId}")]
        public IActionResult GetPokemonByCategoryId(int categoryId)
        {
            var pokemons = _mapper.Map<List<PokemonDto>>(
               _categoryRepository.GetPokemonByCategory(categoryId));

            return Ok(pokemons);
        }

        // =============================
        // CREATE
        // =============================
        [Authorize(Policy = "Category.Add")]
        [HttpPost]
        public IActionResult CreateCategory([FromBody] CategoryDtoCreate categoryCreate)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var allCategories = _categoryRepository.GetCategoriesIncludingDeleted();
            var userId = int.Parse(User.FindFirst("userId").Value);

            var activeCategory = allCategories
                .FirstOrDefault(c =>
                    c.Name.Trim().ToUpper() == categoryCreate.Name.Trim().ToUpper()
                    && !c.IsDeleted);

            if (activeCategory != null)
                return Conflict("Category already exists");

            var deletedCategory = allCategories
                .FirstOrDefault(c =>
                    c.Name.Trim().ToUpper() == categoryCreate.Name.Trim().ToUpper()
                    && c.IsDeleted);

            if (deletedCategory != null)
            {
                deletedCategory.IsDeleted = false;
                deletedCategory.DeletedDateTime = null;
                deletedCategory.DeletedUserId = null;

                deletedCategory.UpdatedUserId = userId;
                deletedCategory.UpdatedDateTime = DateTime.Now;

                _categoryRepository.UpdateCategory(deletedCategory, userId);
                return Ok("Category restored from deleted list.");
            }

            var newCategory = _mapper.Map<Category>(categoryCreate);

            if (!_categoryRepository.CreateCategory(newCategory, userId))
                return StatusCode(500, "Something went wrong while saving");

            return CreatedAtAction(nameof(GetCategory),
                new { categoryId = newCategory.Id },
                newCategory);
        }

        // =============================
        // UPDATE
        // =============================
        [Authorize(Policy = "Category.Update")]
        [HttpPut("{categoryId}")]
        public IActionResult UpdateCategory(int categoryId, [FromBody] CategoryDto updatedCategory)
        {
            if (!ModelState.IsValid || updatedCategory == null)
                return BadRequest(ModelState);

            if (categoryId != updatedCategory.Id)
                return BadRequest("Id mismatch");

            if (!_categoryRepository.CategoryExist(categoryId))
                return NotFound();

            var allCategories = _categoryRepository.GetCategories();
            var userId = int.Parse(User.FindFirst("userId").Value);

            bool duplicateExists = DuplicateCheckHelper.ExistsDuplicate(
                allCategories,
                c => c.Name,
                updatedCategory.Name,
                categoryId,
                c => c.Id
            );

            if (duplicateExists)
                return Conflict("A category with the same name already exists");

            var existingCategory = _categoryRepository.GetCategory(categoryId);
            existingCategory.Name = updatedCategory.Name;

            if (!_categoryRepository.UpdateCategory(existingCategory, userId))
                return StatusCode(500, "Something went wrong while updating");

            return NoContent();
        }

        // =============================
        // DELETE (SOFT)
        // =============================
        [Authorize(Policy = "Category.Delete")]
        [HttpDelete("{id}")]
        public IActionResult SoftDeleteCategory(int id)
        {
            // 1) Kategori var mı?
            var category = _categoryRepository.GetCategory(id);
            if (category == null)
                return NotFound("Category not found");

            // 2) User ID token'dan alınır
            var userId = int.Parse(User.FindFirst("userId").Value);

            // 3) Repository metodu Category nesnesi bekliyor
            if (!_categoryRepository.SoftDeleteCategory(id, userId))
                return StatusCode(500, "Failed to delete category");

            return NoContent();
        }


        // =============================
        // RESTORE
        // =============================
        [Authorize(Policy = "Category.Restore")]
        [HttpPost("restore/{id}")]
        public IActionResult RestoreCategory(int id)
        {
            var category = _categoryRepository.GetCategoryIncludingDeleted(id);
            if (category == null)
                return NotFound("Category not found");

            // Duplicate aktif kategori kontrolü
            var duplicateActive = _categoryRepository.GetCategoriesIncludingDeleted()
                .Any(c => c.Name.Trim().ToUpper() == category.Name.Trim().ToUpper()
                       && !c.IsDeleted
                       && c.Id != id);

            if (duplicateActive)
                return Conflict("There is already an active category with this name.");

            var userId = int.Parse(User.FindFirst("userId").Value);

            category.IsDeleted = false;
            category.DeletedDateTime = null;
            category.DeletedUserId = null;

            _categoryRepository.UpdateCategory(category, userId);

            return Ok("Category restored successfully!");
        }

        // =============================
        // DELETED LIST
        // =============================
        [Authorize(Policy = "Category.ListDeleted")]
        [HttpGet("deleted")]
        public IActionResult GetDeletedCategories()
        {
            var deletedCategories = _mapper.Map<List<CategoryDto>>(
                _categoryRepository.GetDeletedCategories());

            return Ok(deletedCategories);
        }
    }
}
