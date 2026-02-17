namespace menu_users.Application.DTOs.Auth;

public record RegisterUserRequest
(
    string FirstName,
    string LastName,
    string Email,
    string Password,
    int RoleId
);
