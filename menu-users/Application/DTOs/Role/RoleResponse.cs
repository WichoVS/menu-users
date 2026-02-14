namespace menu_users.Application.DTOs.Role;

public record RoleResponse
(
    int Id,
    string Name,
    int Hierarchy,
    bool IsActive,
    DateTime CreatedAt,
    DateTime UpdatedAt
);
