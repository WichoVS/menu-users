
using System;

namespace menu_users.Domain.Interfaces.Users;

public interface IPasswordHasher
{
    string Hash(string password);
    bool Verify(string hashedPassword, string providedPassword);
}
