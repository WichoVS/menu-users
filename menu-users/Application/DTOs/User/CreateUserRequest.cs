namespace menu_users.Application.DTOs.User;

public record CreateUserRequest
(
    string FirstName,
    string LastName,
    string Email,
    int RoleId
);