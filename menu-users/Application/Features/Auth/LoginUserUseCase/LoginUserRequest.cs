namespace menu_users.Application.DTOs.Auth;

public record LoginUserRequest
(
    string Email,
    string Password
);