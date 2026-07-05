using FluentResults;

namespace ToDoApp.Application.Errors
{
    public class UnauthorizedError : Error
    {
        public UnauthorizedError(string message) : base(message) { }
    }
}
