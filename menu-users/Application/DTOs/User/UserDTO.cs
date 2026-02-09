namespace menu_users.Application.DTOs.User;

public record class UserDTO
(
    string Id,
    string FirstName,
    string LastName,
    string Email,
    string RoleName,
    int RoleId
);