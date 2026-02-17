namespace menu_users.Application.Features.Roles.UpdateRoleUseCase;

public record UpdateRoleRequest
(
    string Name,
    int Hierarchy
);