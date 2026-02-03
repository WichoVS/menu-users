namespace menu_users.Application.DTOs.Role;

public record class RoleCreateRequest
(
    string Name,
    int Hierarchy
);
