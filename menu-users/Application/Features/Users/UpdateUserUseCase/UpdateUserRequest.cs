namespace menu_users.Application.Features.Users.UpdateUserUseCase;

public record UpdateUserRequest
(
    string FirstName,
    string LastName,
    string Email,
    int RoleId
);