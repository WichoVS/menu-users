namespace menu_users.Application.DTOs.Auth;

public record class SignInRequest
{
    string Nombre;
    string Apellido;
    string Correo;
    string Password;
    Guid RolId;
}
