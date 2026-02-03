namespace menu_users.Application.DTOs.Menu;

public record class MenuDTO
(
    int IdMenu,
    string Name,
    string Route,
    string IsMain,
    int? ParentId,
    MenuDTO[] Children
);
