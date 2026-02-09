namespace menu_users.Application.DTOs.Menu;

public record class MenuDTO
(
    int IdMenu,
    string Name,
    string Url,
    bool IsMain,
    int? ParentId,
    int MinHierarchy,
    MenuDTO[] Children
);
