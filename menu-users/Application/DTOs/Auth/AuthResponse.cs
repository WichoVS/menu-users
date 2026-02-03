namespace menu_users.Application.DTOs.Auth;

public record AuthResponse
(
    string id,
    string firstName,
    string lastName,
    string email,
    string role,
    string token
);