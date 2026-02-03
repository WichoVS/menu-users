namespace menu_users.Application.DTOs.Role;

public record class RoleUpdateRequest
(
    string Name,
    int Hierarchy
);
