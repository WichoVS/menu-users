namespace menu_users.Application.DTOs.Auth;

public record class SignInRequest
(
    string FirstName,
    string LastName,
    string Email,
    string Password,
    int RoleId
);
