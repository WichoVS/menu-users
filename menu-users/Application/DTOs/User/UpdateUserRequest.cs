namespace menu_users.Application.DTOs.User;

public record UpdateUserRequest
(
    string FirstName,
    string LastName,
    string Email,
    int RoleId
);