using System.Security.Claims;
using Clicker.BL.Abstractions;
using Microsoft.AspNetCore.Http;
namespace Clicker.BL.Implementations;

public class CurrentUserService : ICurrentUserService {
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor) {
        _httpContextAccessor = httpContextAccessor;
    }

    public string? AccessTokenRaw {
        get {
               
            var authorizationHeader = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString();
            if (!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase)) {
                return authorizationHeader["Bearer ".Length..].Trim();
            }
            return null;
        }
    }
    public string? UserId =>
        _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

    public string? TelegramId =>
        _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Name);

    public string? UserRole =>
        _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Role);
}