using FluentResults;
using ToDoApp.Application.DTOs.Auth;

namespace ToDoApp.Application.Services.Interfaces
{
    public interface IAuthService
    {
        Task<Result<AuthResponseDto>> RegisterAsync(RegisterRequestDto dto);
        Task<Result<AuthResponseDto>> LoginAsync(LoginRequestDto dto);
        Task<Result<AuthResponseDto>> RefreshTokenAsync(RefreshTokenRequestDto dto);
        Task<Result> LogoutAsync(int userId);
    }
}
