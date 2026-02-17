namespace menu_users.Application.Features.Menus.UpdateMenuUseCase;

public record UpdateMenuRequest
(
    string Name,
    bool IsMain,
    int? ParentId,
    string Url,
    int MinHierarchy
);
