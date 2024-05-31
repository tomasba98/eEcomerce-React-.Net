namespace eEcomerce.BackEnd.Services.Category;

using eEcomerce.BackEnd.Entities.Category;
public interface ICategoryService
{
    Category CreateCategory(Category category);

    Category GetCategoryById(Guid categoryId);

    Category GetCategoryByName(string categoryName);

    Category GetCategoryByLetter(char categoryLeter);

    Task<bool> UpdateCategory(Category category);

    Task<bool> DeleteCategory(Category category);

    IEnumerable<Category> GetAllCategories();
}
