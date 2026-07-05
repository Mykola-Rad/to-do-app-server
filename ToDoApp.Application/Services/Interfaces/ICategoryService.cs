using FluentResults;
using ToDoApp.Application.DTOs.Categories;

namespace ToDoApp.Application.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<Result<IEnumerable<CategoryDto>>> GetForUserAsync(int userId);
        Task<Result<CategoryDto>> CreateAsync(int userId, CreateCategoryDto dto);
        Task<Result> DeleteAsync(int id, int userId);
    }
}
