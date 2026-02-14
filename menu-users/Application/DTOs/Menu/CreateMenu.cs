namespace menu_users.Application.DTOs.Menu;

public record CreateMenu
(
    string Name,
    bool IsMain,
    int? ParentId,
    string Url,
    int MinHierarchy
);
