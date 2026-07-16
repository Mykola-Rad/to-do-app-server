using FluentResults;
using Microsoft.Extensions.Logging;
using ToDoApp.Application.DTOs.Auth;
using ToDoApp.Application.Errors;
using ToDoApp.Application.Interfaces;
using ToDoApp.Application.Services.Interfaces;
using ToDoApp.Domain.Entities;
using ToDoApp.Domain.Repositories;

namespace ToDoApp.Application.Services.Realizations
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenService _tokenService;
        private readonly ILogger<AuthService> _logger;

        public AuthService(
            IUnitOfWork unitOfWork,
            IPasswordHasher passwordHasher,
            ITokenService tokenService,
            ILogger<AuthService> logger)
        {
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
            _logger = logger;
        }

        public async Task<Result<AuthResponseDto>> RegisterAsync(RegisterRequestDto dto)
        {
            var existingUser = await _unitOfWork.Users.GetByEmailAsync(dto.Email);
            if (existingUser != null)
            {
                _logger.LogWarning("Registration failed: User with email {Email} already exists.", dto.Email);
                return Result.Fail<AuthResponseDto>(new BadRequestError("User with this email already exists."));
            }

            var refreshToken = _tokenService.GenerateRefreshToken();

            var user = new User
            {
                Email = dto.Email,
                PasswordHash = _passwordHasher.HashPassword(dto.Password),
                RefreshToken = refreshToken,
                RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7)
            };

            user.Categories = new List<Category>
            {
                new() { Name = "Work", ColorHex = "#4A90E2" },
                new() { Name = "Personal", ColorHex = "#2ECC71" },
                new() { Name = "Learning", ColorHex = "#9B59B6" },
                new() { Name = "Sports", ColorHex = "#E67E22" }
            };

            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();

            var accessToken = _tokenService.GenerateAccessToken(user);

            var response = new AuthResponseDto(accessToken, refreshToken, user.Email);
            return Result.Ok(response);
        }

        public async Task<Result<AuthResponseDto>> LoginAsync(LoginRequestDto dto)
        {
            var user = await _unitOfWork.Users.GetByEmailAsync(dto.Email);
            if (user == null || !_passwordHasher.VerifyPassword(dto.Password, user.PasswordHash))
            {
                _logger.LogWarning($"Failed login attempt!");

                return Result.Fail<AuthResponseDto>(
                    new UnauthorizedError("Incorrect email or password."));
            }

            var accessToken = _tokenService.GenerateAccessToken(user);
            var refreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

            await _unitOfWork.SaveChangesAsync();

            var response = new AuthResponseDto(accessToken, refreshToken, user.Email);
            return Result.Ok(response);
        }

        public async Task<Result<AuthResponseDto>> RefreshTokenAsync(RefreshTokenRequestDto dto)
        {
            try
            {
                var principal = _tokenService.GetPrincipalFromExpiredToken(dto.AccessToken);
                var email = principal.Identity?.Name;

                if (string.IsNullOrEmpty(email))
                {
                    _logger.LogWarning("Token refresh failed: Claims principal email is empty.");
                    return Result.Fail<AuthResponseDto>(new BadRequestError("Invalid token."));
                }

                var user = await _unitOfWork.Users.GetByEmailAsync(email);
                if (user == null)
                {
                    return Result.Fail<AuthResponseDto>(new UnauthorizedError("User not found."));
                }

                if (!string.IsNullOrEmpty(user.PreviousRefreshToken) && dto.RefreshToken == user.PreviousRefreshToken)
                {
                    _logger.LogCritical("SECURITY BREACH DETECTED for user '{Email}': " +
                                        "A previously used refresh token was presented! Revoking session.", email);

                    user.RefreshToken = null;
                    user.PreviousRefreshToken = null;
                    user.RefreshTokenExpiryTime = null;
                    await _unitOfWork.SaveChangesAsync();

                    return Result.Fail<AuthResponseDto>(
                        new UnauthorizedError("Security breach detected. All sessions revoked. Please log in again."));
                }

                if (dto.RefreshToken != user.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
                {
                    _logger.LogWarning("Token refresh failed for user '{Email}': " +
                                       "Invalid refresh token or token has expired.", email);
                    return Result.Fail<AuthResponseDto>(
                        new UnauthorizedError("Invalid refresh token or token has expired."));
                }

                var newAccessToken = _tokenService.GenerateAccessToken(user);
                var newRefreshToken = _tokenService.GenerateRefreshToken();

                user.PreviousRefreshToken = user.RefreshToken;
                user.RefreshToken = newRefreshToken;
                user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

                await _unitOfWork.SaveChangesAsync();

                var response = new AuthResponseDto(newAccessToken, newRefreshToken, user.Email);
                return Result.Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Token refresh failed with an exception during token parsing.");
                return Result.Fail<AuthResponseDto>(
                    new BadRequestError("It was not possible to process the access token."));
            }
        }

        public async Task<Result> LogoutAsync(int userId)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null)
            {
                _logger.LogWarning("Logout failed: User with ID {UserId} was not found.", userId);
                return Result.Fail(new NotFoundError("User was not found."));
            }

            user.RefreshToken = null;
            user.PreviousRefreshToken = null;
            user.RefreshTokenExpiryTime = null;

            await _unitOfWork.SaveChangesAsync();
            return Result.Ok();
        }
    }
}