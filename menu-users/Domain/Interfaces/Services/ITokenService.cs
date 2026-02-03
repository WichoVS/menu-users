using System;
using menu_users.Domain.Entities;

namespace menu_users.Domain.Interfaces.Repositories;

public interface ITokenService
{
    string GenerateToken(User user, Role role);
}
