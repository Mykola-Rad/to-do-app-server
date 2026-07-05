using FluentResults;

namespace ToDoApp.Application.Errors
{
    public class NotFoundError : Error
    {
        public NotFoundError(string message) : base(message) { }
    }
}
