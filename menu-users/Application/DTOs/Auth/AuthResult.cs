namespace menu_users.Application.DTOs.Auth;

public record AuthResult
(
    bool Success,
    string? Error,
    AuthResponse? Data
);
