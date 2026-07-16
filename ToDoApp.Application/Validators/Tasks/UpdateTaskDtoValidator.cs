using FluentValidation;
using ToDoApp.Application.DTOs.Tasks;

namespace ToDoApp.Application.Validators.Tasks
{
    public class UpdateTaskDtoValidator : AbstractValidator<UpdateTaskDto>
    {
        public UpdateTaskDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Task title is required.")
                .MaximumLength(200).WithMessage("Task title cannot exceed 200 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(1000).WithMessage("Task description cannot exceed 1000 characters.");

            RuleFor(x => x.CategoryId)
                .GreaterThan(0).WithMessage("You must choose a valid category.");
        }
    }
}
