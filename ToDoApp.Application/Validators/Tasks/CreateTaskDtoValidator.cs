using FluentValidation;
using ToDoApp.Application.DTOs.Tasks;

namespace ToDoApp.Application.Validators.Tasks
{
    public class CreateTaskDtoValidator : AbstractValidator<CreateTaskDto>
    {
        public CreateTaskDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Task title is required.")
                .MaximumLength(200).WithMessage("Task title cannot exceed 200 characters.");

            RuleFor(x => x.CategoryId)
                .GreaterThan(0).WithMessage("You must choose a valid category.");

            RuleFor(x => x.DueDate)
                .Must(date => !date.HasValue || date.Value.Date >= DateTime.UtcNow.Date)
                .WithMessage("Due date cannot be in the past.");
        }
    }
}
