namespace menu_users.Application.DTOs.Auth;

public record class AuthResult
(
    bool Success,
    string? Error,
    AuthResponse? Data
);
