namespace menu_users.Application.DTOs.Menu;

public record class CreateMenu
(
    string Name,
    bool IsMain,
    int? ParentId,
    string Url,
    int MinHierarchy
);
