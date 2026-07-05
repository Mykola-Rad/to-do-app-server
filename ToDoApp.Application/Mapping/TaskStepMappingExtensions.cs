using ToDoApp.Application.DTOs.Tasks.Steps;
using ToDoApp.Domain.Entities;

namespace ToDoApp.Application.Mapping
{
    public static class TaskStepMappingExtensions
    {
        public static TaskStepResponseDto ToDto(this ToDoTaskStep step)
        {
            if (step == null) return null!;

            return new TaskStepResponseDto(
                step.Id,
                step.Title,
                step.IsCompleted
            );
        }

        public static ToDoTaskStep ToEntity(this AddStepDto dto, int taskId)
        {
            return new ToDoTaskStep
            {
                Title = dto.Title,
                ToDoTaskId = taskId,
                IsCompleted = false
            };
        }
    }
}
