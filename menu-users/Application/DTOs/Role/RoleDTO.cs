namespace menu_users.Application.DTOs.Role;

public record RoleDTO
(
    int Id,
    string Name,
    int Hierarchy,
    bool IsActive,
    DateTime CreatedAt,
    DateTime UpdatedAt
);
