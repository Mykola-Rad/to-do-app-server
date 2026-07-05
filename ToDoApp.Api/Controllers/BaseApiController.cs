using FluentResults;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ToDoApp.Application.Errors;

namespace ToDoApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class BaseApiController : ControllerBase
    {
        protected int CurrentUserId
        {
            get
            {
                var nameIdentifier = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!int.TryParse(nameIdentifier, out var userId))
                {
                    throw new InvalidOperationException("User ID claim is missing or invalid in the current token.");
                }

                return userId;
            }
        }

        protected IActionResult ProcessResult<T>(Result<T> result)
        {
            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return MatchErrorToStatusCode(result.Errors);
        }

        protected IActionResult ProcessResult(Result result)
        {
            if (result.IsSuccess)
            {
                return Ok();
            }

            return MatchErrorToStatusCode(result.Errors);
        }

        private IActionResult MatchErrorToStatusCode(IEnumerable<IError> errors)
        {
            var firstError = errors.FirstOrDefault();

            return firstError switch
            {
                NotFoundError => NotFound(new { message = firstError.Message }),
                UnauthorizedError => Unauthorized(new { message = firstError.Message }),
                BadRequestError => BadRequest(new { message = firstError.Message }),

                _ => BadRequest(new { message = firstError?.Message ?? "Unknown error" })
            };
        }
    }
}
