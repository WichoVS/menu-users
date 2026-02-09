using menu_users.Application.DTOs.Menu;

namespace menu_users.Application.DTOs.MenuToUser;

public record class MenuToUserDTO
(
    int IdMenuToUser,
    Guid UserId,
    MenuDTO[] Menu
);
