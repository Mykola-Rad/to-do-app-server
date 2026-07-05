using FluentValidation;
using ToDoApp.Application.DTOs.Tasks.Steps;

namespace ToDoApp.Application.Validators.Tasks.Steps
{
    public class AddStepDtoValidator : AbstractValidator<AddStepDto>
    {
        public AddStepDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Step title is required.")
                .MaximumLength(150).WithMessage("Step title cannot exceed 150 characters.");
        }
    }
}
