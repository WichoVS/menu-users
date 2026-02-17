namespace menu_users.Application.Features.Users.CreateUserUseCase;

public record CreateUserRequest
(
    string FirstName,
    string LastName,
    string Email,
    int RoleId
);