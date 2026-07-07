using FluentValidation;
using ToDoApp.Application.DTOs.Tasks;

namespace ToDoApp.Application.Validators.Tasks
{
    public class DeleteTasksDtoValidator : AbstractValidator<DeleteTasksDto>
    {
        public DeleteTasksDtoValidator()
        {
            RuleFor(x => x.Ids)
                .NotNull().WithMessage("Task IDs list cannot be null.")
                .Must(ids => ids != null && ids.Any()).WithMessage("At least one task ID must be provided for deletion.");
        }
    }
}
