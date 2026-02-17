namespace menu_users.Application.Features.Roles.CreateRoleUseCase;

public record CreateRoleRequest
(
    string Name,
    int Hierarchy
);
