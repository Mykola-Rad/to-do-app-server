namespace ToDoApp.Application.DTOs.Auth
{
    public record AuthResponseDto(string AccessToken, string RefreshToken, string Email);
}
