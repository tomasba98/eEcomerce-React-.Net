namespace eEcomerce.BackEnd.Services.Category;

using eEcomerce.BackEnd.Entities.Category;

/// <summary>
/// Service interface for managing categories.
/// </summary>
public interface ICategoryService
{
    /// <summary>
    /// Creates a new category.
    /// </summary>
    /// <param name="category">The category to create.</param>
    /// <returns>The created category.</returns>
    Category CreateCategory(Category category);

    /// <summary>
    /// Retrieves a category by its unique identifier.
    /// </summary>
    /// <param name="categoryId">The unique identifier of the category.</param>
    /// <returns>The category if found; otherwise, null.</returns>
    Category? GetCategoryById(Guid categoryId);

    /// <summary>
    /// Retrieves a category by its name.
    /// </summary>
    /// <param name="categoryName">The name of the category.</param>
    /// <returns>The category if found; otherwise, null.</returns>
    Category? GetCategoryByName(string categoryName);

    /// <summary>
    /// Retrieves a category by its starting letter.
    /// </summary>
    /// <param name="categoryLeter">The starting letter of the category name.</param>
    /// <returns>The category if found; otherwise, null.</returns>
    Category? GetCategoryByLetter(char categoryLeter);

    /// <summary>
    /// Updates an existing category asynchronously.
    /// </summary>
    /// <param name="category">The category to update.</param>
    /// <returns>True if the category was updated successfully; otherwise, false.</returns>
    Task<bool> UpdateCategory(Category category);

    /// <summary>
    /// Deletes an existing category asynchronously.
    /// </summary>
    /// <param name="category">The category to delete.</param>
    /// <returns>True if the category was deleted successfully; otherwise, false.</returns>
    Task<bool> DeleteCategory(Category category);

    /// <summary>
    /// Retrieves all categories.
    /// </summary>
    /// <returns>A collection of all categories.</returns>
    IEnumerable<Category> GetAllCategories();
}

