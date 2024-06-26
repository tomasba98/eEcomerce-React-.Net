﻿using eEcomerce.BackEnd.Services.DataAccessLayer;

namespace eEcomerce.BackEnd.Services.Category.Implementation;
using eEcomerce.BackEnd.Entities.Category;
using eEcomerce.BackEnd.Services.Category;

using Microsoft.CodeAnalysis;

using System.Collections.Generic;
using System.Threading.Tasks;

public class CategoryService : ICategoryService
{
    private readonly IGenericService<Category> _categoryGenericService;

    public CategoryService(IGenericService<Category> categoryGenericService)
    {
        _categoryGenericService = categoryGenericService;
    }

    public Category CreateCategory(Category category)
    {
        _categoryGenericService.InsertAsync(category);
        return category;
    }

    public IEnumerable<Category> GetAllCategories()
    {
        IEnumerable<Category> categories = _categoryGenericService.FindAllAsync().Result;
        return categories.ToList();
    }

    public Category? GetCategoryById(Guid categoryId)
    {
        return _categoryGenericService.FilterByExpressionLinq(c => c.Id == categoryId).FirstOrDefault();
    }

    public Category? GetCategoryByName(string categoryName)
    {
        return _categoryGenericService.FilterByExpressionLinq(c => c.Name == categoryName).FirstOrDefault();
    }

    public Category? GetCategoryByLetter(char categoryLetter)
    {
        return _categoryGenericService.FilterByExpressionLinq(c => c.Letter == categoryLetter).FirstOrDefault();
    }

    public async Task<bool> UpdateCategory(Category category)
    {
        try
        {
            await _categoryGenericService.UpdateAsync(category);
            return true;
        }
        catch
        {
            return false;
        }
    }
    public async Task<bool> DeleteCategory(Category category)
    {
        try
        {
            await _categoryGenericService.DeleteAsync(category);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
