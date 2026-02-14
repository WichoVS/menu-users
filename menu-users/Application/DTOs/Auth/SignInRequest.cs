namespace menu_users.Application.DTOs.Auth;

public record SignInRequest
(
    string FirstName,
    string LastName,
    string Email,
    string Password,
    int RoleId
);
