using eEcomerce.BackEnd.Controllers;
using eEcomerce.BackEnd.Entities.Category;
using eEcomerce.BackEnd.Models.Category;
using eEcomerce.BackEnd.Services.Category;

using Microsoft.AspNetCore.Mvc;

using Moq;

namespace UnitTestingeEcomerce.Tests.ControllersTests;
public class CategoryControllerTests
{
    private readonly Mock<ICategoryService> _mockCategoryService;
    private readonly CategoryController _controller;

    public CategoryControllerTests()
    {
        _mockCategoryService = new Mock<ICategoryService>();
        _controller = new CategoryController(_mockCategoryService.Object);
    }

    [Fact]
    public void GetCategorysList_ReturnsOkResult_WithListOfCategories()
    {
        //Arrange
        List<Category> categories = new()
        {
            new Category { Id = Guid.NewGuid(), Letter = 'A', Name = "Category A" },
            new Category { Id = Guid.NewGuid(), Letter = 'B', Name = "Category B" }
        };

        _mockCategoryService.Setup(service => service.GetAllCategories()).Returns(categories);

        //Act
        ActionResult<IEnumerable<Category>> result = _controller.GetCategorysList();

        //Assert
        OkObjectResult okResult = Assert.IsType<OkObjectResult>(result.Result);
        List<CategoryModel> returnValue = Assert.IsType<List<CategoryModel>>(okResult.Value);

        Assert.Equal(2, returnValue.Count);
        Assert.Equal("Category A", returnValue[0].Name);
        Assert.Equal("Category B", returnValue[1].Name);
    }

    [Fact]
    public void GetCategorysList_ReturnsEmptyList_WhenNoCategoriesExist()
    {
        // Arrange
        List<Category> categories = new();
        _mockCategoryService.Setup(service => service.GetAllCategories()).Returns(categories);

        // Act
        ActionResult<IEnumerable<Category>> result = _controller.GetCategorysList();

        // Assert
        OkObjectResult okResult = Assert.IsType<OkObjectResult>(result.Result);
        List<CategoryModel> returnValue = Assert.IsType<List<CategoryModel>>(okResult.Value);

        Assert.Empty(returnValue);
    }
}
