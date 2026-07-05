namespace ToDoApp.Application.DTOs.Common
{
    public record PagedResponseDto<T>(IEnumerable<T> Items, int TotalCount, int PageNumber, int PageSize);
}
