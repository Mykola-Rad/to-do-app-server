using FluentValidation;
using ToDoApp.Application.DTOs.Categories;

namespace ToDoApp.Application.Validators.Categories
{
    public class CreateCategoryDtoValidator : AbstractValidator<CreateCategoryDto>
    {
        public CreateCategoryDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Category name is required.")
                .MaximumLength(50).WithMessage("Category name cannot exceed 50 characters.");

            RuleFor(x => x.ColorHex)
                .NotEmpty().WithMessage("Category color is required.")
                .Matches("^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$")
                .WithMessage("Category color must be in a valid HEX format (e.g., #FF5733).");
        }
    }
}
