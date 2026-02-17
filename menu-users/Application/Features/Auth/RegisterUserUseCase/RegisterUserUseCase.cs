using System;
using menu_users.Application.DTOs.Auth;
using menu_users.Domain.Interfaces.Features.Auth;
using menu_users.Domain.Interfaces.Repositories;
using menu_users.Domain.Interfaces.Services;
using menu_users.Domain.Interfaces.Users;
using menu_users.Domain.Entities;
using menu_users.Application.DTOs.Menu;

namespace menu_users.Application.Features.Auth.RegisterUser;

public class RegisterUserUseCase : IRegisterUserUseCase
{
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IMenuRepository _menuRepository;
    private readonly IAccessAssignmentService _accessAssignmentService;

    public RegisterUserUseCase(
        IPasswordHasher passwordHasher,
        ITokenService tokenService,
        IUserRepository userRepository,
        IRoleRepository roleRepository,
        IMenuRepository menuRepository,
        IAccessAssignmentService accessAssignmentService)
    {
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _menuRepository = menuRepository;
        _accessAssignmentService = accessAssignmentService;
    }

    public async Task<AuthResult> Execute(RegisterUserRequest request)
    {
        User? existingUser = await _userRepository.GetByEmailAsync(request.Email);
        if (existingUser != null)
        {
            return new AuthResult(false, "El correo electrónico ya está en uso", null);
        }

        Role? role = await _roleRepository.GetLowestByHierarchyAsync();
        if (role == null || role.IsActive == false)
        {
            return new AuthResult(false, "Rol no válido", null);
        }

        var hashedPassword = _passwordHasher.Hash(request.Password);
        var newUser = new User()
        {
            Id = Guid.NewGuid(),
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            Password = hashedPassword,
            RoleId = role.Id,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            IsActive = true
        };

        await _userRepository.AddAsync(newUser);


        // Por tiempo esto lo dejaré así pero esta logica lo mejor es moverla a AssignDefaultMenusToUserUseCase, para que quede toda la logica de asignación de menus por defecto en un solo lugar,
        //  y no tener esta logica mezclada en el registro del usuario
        // Ahora obtenemos todos los menus a los que el rol del usuario tiene acceso
        IEnumerable<Menu> accessibleMenus = await _menuRepository.GetMenusByUserHierarchyAsync(role.Hierarchy);
        if (!accessibleMenus.Any())
        {
            return new AuthResult(false, "No accessible menus for the user's hierarchy", null);
        }

        // Flatten la lista de menús y submenús para asignarlos al usuario
        List<MenuDTO> menusToAssign = new List<MenuDTO>();

        foreach (var menu in accessibleMenus)
        {
            MenuDTO menuToAssign = new MenuDTO
            (
                IdMenu: menu.Id,
                Name: menu.Name,
                Url: menu.Url,
                IsMain: menu.IsMain,
                ParentId: menu.ParentMenuId,
                MinHierarchy: menu.MinimumHierarchy,
                Children: Array.Empty<MenuDTO>()
            );

            menusToAssign.Add(menuToAssign);

            if (menu.SubMenus != null && menu.SubMenus.Any())
            {
                foreach (var subMenu in menu.SubMenus)
                {
                    MenuDTO subMenuToAssign = new MenuDTO
                    (
                        IdMenu: subMenu.Id,
                        Name: subMenu.Name,
                        Url: subMenu.Url,
                        IsMain: subMenu.IsMain,
                        ParentId: subMenu.ParentMenuId,
                        MinHierarchy: subMenu.MinimumHierarchy,
                        Children: Array.Empty<MenuDTO>()
                    );

                    menusToAssign.Add(subMenuToAssign);
                }
            }
        }

        await _accessAssignmentService.AssignDefaultMenusToUserAsync(newUser.Id.ToString(), menusToAssign);

        var token = _tokenService.GenerateToken(newUser, role);

        var authResponse = new AuthResponse
        (
            token: token,
            id: newUser.Id.ToString(),
            email: newUser.Email,
            firstName: newUser.FirstName,
            lastName: newUser.LastName,
            role: role.Name
        );

        return new AuthResult(true, null, authResponse);
    }
}
