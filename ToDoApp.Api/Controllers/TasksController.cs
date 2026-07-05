using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDoApp.Application.DTOs.Common;
using ToDoApp.Application.DTOs.Tasks;
using ToDoApp.Application.DTOs.Tasks.Steps;
using ToDoApp.Application.Services.Interfaces;

namespace ToDoApp.Api.Controllers;

[Authorize]
[ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorResponseDto))]
public class TasksController : BaseApiController
{
    private readonly ITaskService _taskService;

    public TasksController(
        ITaskService taskService)
    {
        _taskService = taskService;
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TaskResponseDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponseDto))]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _taskService.GetByIdAsync(id, CurrentUserId);
        return ProcessResult(result);
    }

    [HttpGet("paged")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedResponseDto<TaskResponseDto>))]
    public async Task<IActionResult> GetPaged(
        [FromQuery] int? categoryId,
        [FromQuery] string? searchTerm,
        [FromQuery] bool? isToday,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await _taskService.GetPagedAsync(
            CurrentUserId, categoryId, searchTerm, isToday, pageNumber, pageSize);
        return ProcessResult(result);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TaskResponseDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponseDto))]
    public async Task<IActionResult> Create([FromBody] CreateTaskDto dto)
    {
        var result = await _taskService.CreateAsync(CurrentUserId, dto);
        return ProcessResult(result);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TaskResponseDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponseDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponseDto))]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateTaskDto dto)
    {
        var result = await _taskService.UpdateAsync(id, CurrentUserId, dto);
        return ProcessResult(result);
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)] 
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponseDto))]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _taskService.DeleteAsync(id, CurrentUserId);
        return ProcessResult(result);
    }

    [HttpPatch("{id:int}/toggle")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponseDto))]
    public async Task<IActionResult> ToggleTask(int id)
    {
        var result = await _taskService.ToggleTaskAsync(id, CurrentUserId);
        return ProcessResult(result);
    }


    [HttpPost("{id:int}/steps")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TaskStepResponseDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponseDto))]
    public async Task<IActionResult> AddStep(int id, [FromBody] AddStepDto dto)
    {
        var result = await _taskService.AddStepAsync(id, CurrentUserId, dto);
        return ProcessResult(result);
    }

    [HttpDelete("{id:int}/steps/{stepId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)] 
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponseDto))]
    public async Task<IActionResult> RemoveStep(int id, int stepId)
    {
        var result = await _taskService.RemoveStepAsync(id, stepId, CurrentUserId);
        return ProcessResult(result);
    }

    [HttpPatch("{id:int}/steps/{stepId:int}/toggle")]
    [ProducesResponseType(StatusCodes.Status200OK)] 
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponseDto))]
    public async Task<IActionResult> ToggleStep(int id, int stepId)
    {
        var result = await _taskService.ToggleStepAsync(id, stepId, CurrentUserId);
        return ProcessResult(result);
    }
}