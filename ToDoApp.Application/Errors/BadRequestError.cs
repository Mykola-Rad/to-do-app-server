using FluentResults;

namespace ToDoApp.Application.Errors
{
    public class BadRequestError : Error
    {
        public BadRequestError(string message) : base(message) { }
    }
}
