namespace ToDoApp.Application.DTOs.Auth
{
    public record RefreshTokenRequestDto(string AccessToken, string RefreshToken);
}
