namespace menu_users.Application.DTOs.User;

public record class CreateUserRequest
(
    string FirstName,
    string LastName,
    string Email,
    int RoleId
);