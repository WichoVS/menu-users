using menu_users.Application.DTOs.Menu;

namespace menu_users.Application.DTOs.MenuToUser;

public record class MenuToUserDTO
(
    Guid UserId,
    MenuDTO[] Menu
);
