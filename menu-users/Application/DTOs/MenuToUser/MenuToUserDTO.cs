using menu_users.Application.DTOs.Menu;

namespace menu_users.Application.DTOs.MenuToUser;

public record MenuToUserDTO
(
    Guid UserId,
    MenuDTO[] Menu
);