namespace menu_users.Application.Features.Menus.CreateMenuUseCase;

public record CreateMenuRequest
(
    string Name,
    string Url,
    bool IsMain,
    int? ParentId,
    int MinHierarchy
);
