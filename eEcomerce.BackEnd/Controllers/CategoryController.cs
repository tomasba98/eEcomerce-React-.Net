using eEcomerce.BackEnd.Entities.Category;
using eEcomerce.BackEnd.Models.Category;
using eEcomerce.BackEnd.Services.Category;

using Microsoft.AspNetCore.Mvc;

namespace eEcomerce.BackEnd.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    private static CategoryModel MapToDto(Category category)
    {
        return new CategoryModel
        {
            Id = category.Id,
            Letter = category.Letter,
            Name = category.Name,

        };
    }

    [HttpGet]
    public ActionResult<IEnumerable<Category>> GetCategorysList()
    {
        IEnumerable<Category> categories = _categoryService.GetAllCategories();
        IEnumerable<CategoryModel> result = categories.Select(category => MapToDto(category)).ToList();

        return Ok(result);
    }
}

