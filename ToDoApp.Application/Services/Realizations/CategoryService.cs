using FluentResults;
using Microsoft.Extensions.Logging;
using ToDoApp.Application.DTOs.Categories;
using ToDoApp.Application.Errors;
using ToDoApp.Application.Mapping;
using ToDoApp.Application.Services.Interfaces;
using ToDoApp.Domain.Repositories;

namespace ToDoApp.Application.Services.Realizations;

public class CategoryService : ICategoryService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CategoryService> _logger;

    public CategoryService(IUnitOfWork unitOfWork, ILogger<CategoryService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<IEnumerable<CategoryDto>>> GetForUserAsync(int userId)
    {
        var categories = await _unitOfWork.Categories.GetCategoriesForUserAsync(userId);

        var dtos = categories.Select(c => c.ToDto());

        return Result.Ok(dtos);
    }

    public async Task<Result<CategoryDto>> CreateAsync(int userId, CreateCategoryDto dto)
    {
        var category = dto.ToEntity(userId);

        await _unitOfWork.Categories.AddAsync(category);
        await _unitOfWork.SaveChangesAsync();

        return Result.Ok(category.ToDto());
    }

    public async Task<Result> DeleteAsync(int id, int userId)
    {
        var category = await _unitOfWork.Categories.GetByIdAsync(id);

        if (category == null)
        {
            _logger.LogWarning("Delete category failed: Category with ID " +
                "{CategoryId} was not found.", id);
            return Result.Fail(new NotFoundError("The category was not found."));
        }

        if (category.UserId != userId)
        {
            _logger.LogWarning(
                "Delete category failed: User {UserId} does not have access " +
                "to category {CategoryId}.", userId, id);
            return Result.Fail(new NotFoundError("The category was not found " +
                "or you do not have access."));
        }

        _unitOfWork.Categories.Delete(category);
        await _unitOfWork.SaveChangesAsync();

        return Result.Ok();
    }
}