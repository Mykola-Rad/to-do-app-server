using ToDoApp.Application.DTOs.Categories;
using ToDoApp.Domain.Entities;

namespace ToDoApp.Application.Mapping
{
    public static class CategoryMappingExtensions
    {
        public static CategoryDto ToDto(this Category category)
        {
            if (category == null) return null!;

            return new CategoryDto(
                category.Id,
                category.Name,
                category.ColorHex
            );
        }

        public static Category ToEntity(this CreateCategoryDto dto, int? userId)
        {
            return new Category
            {
                Name = dto.Name,
                ColorHex = dto.ColorHex,
                UserId = userId
            };
        }
    }
}
