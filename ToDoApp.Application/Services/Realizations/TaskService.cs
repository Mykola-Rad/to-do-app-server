using FluentResults;
using Microsoft.Extensions.Logging;
using ToDoApp.Application.DTOs.Common;
using ToDoApp.Application.DTOs.Tasks;
using ToDoApp.Application.DTOs.Tasks.Steps;
using ToDoApp.Application.Errors;
using ToDoApp.Application.Mapping;
using ToDoApp.Application.Services.Interfaces;
using ToDoApp.Domain.Enums;
using ToDoApp.Domain.Repositories;

namespace ToDoApp.Application.Services.Realizations;

public class TaskService : ITaskService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<TaskService> _logger;

    public TaskService(IUnitOfWork unitOfWork, ILogger<TaskService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<TaskResponseDto>> GetByIdAsync(int id, int userId)
    {
        var task = await _unitOfWork.Tasks.GetWithStepsByIdAsync(id, userId);

        if (task == null)
        {
            _logger.LogWarning("Get task failed: Task ID {TaskId} not found or " +
                "access denied for user {UserId}.", id, userId);
            return Result.Fail<TaskResponseDto>(new NotFoundError(
                "The task was not found or you do not have access."));
        }

        return Result.Ok(task.ToDto());
    }

    public async Task<Result<PagedResponseDto<TaskResponseDto>>> GetPagedAsync(
        int userId,
        int? categoryId,
        string? searchTerm,
        TaskFilterType filter,
        int pageNumber,
        int pageSize)
    {
        var tasks = await _unitOfWork.Tasks.GetPagedTasksAsync(
            userId, categoryId, searchTerm, filter, pageNumber, pageSize);

        var totalCount = await _unitOfWork.Tasks.GetTotalCountAsync(
            userId, categoryId, searchTerm, filter);

        var taskDtos = tasks.Select(t => t.ToDto());
        var response = new PagedResponseDto<TaskResponseDto>(taskDtos, totalCount, pageNumber, pageSize);

        return Result.Ok(response);
    }

    public async Task<Result<TaskResponseDto>> CreateAsync(int userId, CreateTaskDto dto)
    {
        var category = await _unitOfWork.Categories.GetByIdAsync(dto.CategoryId);
        if (category == null || category.UserId != userId)
        {
            _logger.LogWarning("Create task failed: Invalid category ID {CategoryId} " +
                "specified by user {UserId}.", dto.CategoryId, userId);
            return Result.Fail<TaskResponseDto>(new BadRequestError(
                "The specified category is incorrect."));
        }

        var task = dto.ToEntity(userId);
        task.Category = category;

        await _unitOfWork.Tasks.AddAsync(task);
        await _unitOfWork.SaveChangesAsync();

        return Result.Ok(task.ToDto());
    }

    public async Task<Result<TaskResponseDto>> UpdateAsync(
        int id, int userId, UpdateTaskDto dto)
    {
        var task = await _unitOfWork.Tasks.GetWithStepsByIdAsync(id, userId);
        if (task == null)
        {
            _logger.LogWarning("Update task failed: Task ID {TaskId} " +
                "not found or access denied for user {UserId}.", id, userId);
            return Result.Fail<TaskResponseDto>(new NotFoundError(
                "The task was not found or you do not have access."));
        }

        var category = await _unitOfWork.Categories.GetByIdAsync(dto.CategoryId);
        if (category == null || category.UserId != userId)
        {
            _logger.LogWarning("Update task failed: Invalid category ID {CategoryId} " +
                "specified by user {UserId}.", dto.CategoryId, userId);
            return Result.Fail<TaskResponseDto>(new BadRequestError(
                "The specified category is incorrect."));
        }

        dto.UpdateEntity(task);
        task.Category = category;

        await _unitOfWork.SaveChangesAsync();
        return Result.Ok(task.ToDto());
    }

    public async Task<Result> ToggleTaskAsync(int id, int userId)
    {
        var task = await _unitOfWork.Tasks.GetByIdAsync(id);
        if (task == null || task.UserId != userId)
        {
            _logger.LogWarning("Toggle task failed: Task ID {TaskId} " +
                "not found or access denied for user {UserId}.", id, userId);
            return Result.Fail(new NotFoundError(
                "The task was not found or you do not have access."));
        }

        task.IsCompleted = !task.IsCompleted;

        await _unitOfWork.SaveChangesAsync();
        return Result.Ok();
    }

    public async Task<Result> DeleteAsync(int id, int userId)
    {
        var task = await _unitOfWork.Tasks.GetByIdAsync(id);
        if (task == null || task.UserId != userId)
        {
            _logger.LogWarning("Delete task failed: Task ID {TaskId} " +
                "not found or access denied for user {UserId}.", id, userId);
            return Result.Fail(new NotFoundError(
                "The task was not found or you do not have access."));
        }

        _unitOfWork.Tasks.Delete(task);
        await _unitOfWork.SaveChangesAsync();
        return Result.Ok();
    }

    public async Task<Result> DeleteMultipleAsync(DeleteTasksDto dto, int userId)
    {
        await _unitOfWork.Tasks.DeleteMultipleAsync(dto.Ids, userId);

        await _unitOfWork.SaveChangesAsync();

        return Result.Ok();
    }

    public async Task<Result<TaskStepResponseDto>> AddStepAsync(
        int taskId, int userId, AddStepDto dto)
    {
        var task = await _unitOfWork.Tasks.GetWithStepsByIdAsync(taskId, userId);
        if (task == null)
        {
            _logger.LogWarning("Add step failed: Task ID {TaskId} " +
                "not found or access denied for user {UserId}.", taskId, userId);
            return Result.Fail<TaskStepResponseDto>(
                new NotFoundError("The task was not found or you do not have access."));
        }

        var newStep = dto.ToEntity(taskId);

        task.Steps.Add(newStep);
        await _unitOfWork.SaveChangesAsync();

        return Result.Ok(newStep.ToDto());
    }

    public async Task<Result> RemoveStepAsync(int taskId, int stepId, int userId)
    {
        var task = await _unitOfWork.Tasks.GetWithStepsByIdAsync(taskId, userId);
        if (task == null)
        {
            _logger.LogWarning("Remove step failed: Task ID {TaskId} " +
                "not found or access denied for user {UserId}.", taskId, userId);
            return Result.Fail(new NotFoundError(
                "The task was not found or you do not have access."));
        }

        var step = task.Steps.FirstOrDefault(s => s.Id == stepId);
        if (step == null)
        {
            _logger.LogWarning("Remove step failed: Step ID {StepId} " +
                "not found in task {TaskId}.", stepId, taskId);
            return Result.Fail(new NotFoundError("The step was not found in this task."));
        }

        task.Steps.Remove(step);
        await _unitOfWork.SaveChangesAsync();
        return Result.Ok();
    }

    public async Task<Result> ToggleStepAsync(int taskId, int stepId, int userId)
    {
        var task = await _unitOfWork.Tasks.GetWithStepsByIdAsync(taskId, userId);
        if (task == null)
        {
            _logger.LogWarning("Toggle step failed: Task ID {TaskId} " +
                "not found or access denied for user {UserId}.", taskId, userId);
            return Result.Fail(new NotFoundError(
                "The task was not found or you do not have access."));
        }

        var step = task.Steps.FirstOrDefault(s => s.Id == stepId);
        if (step == null)
        {
            _logger.LogWarning("Toggle step failed: Step ID {StepId} " +
                "not found in task {TaskId}.", stepId, taskId);
            return Result.Fail(new NotFoundError("The step was not found in this task."));
        }

        step.IsCompleted = !step.IsCompleted;
        await _unitOfWork.SaveChangesAsync();
        return Result.Ok();
    }
}