using System;
using menu_users.Application.DTOs.ApiResponse;
using menu_users.Application.DTOs.Menu;
using menu_users.Application.DTOs.MenuToUser;
using menu_users.Application.DTOs.User;
using menu_users.Domain.Entities;
using menu_users.Domain.Interfaces.Features.User;
using menu_users.Domain.Interfaces.Repositories;
using menu_users.Domain.Interfaces.Services;
using menu_users.Domain.Interfaces.Users;

namespace menu_users.Application.Features.Users.CreateUserUseCase;

public class CreateUserUseCase : ICreateUserUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IAccessAssignmentService _accessAssignmentService;
    private readonly IMenuRepository _menuRepository;

    public CreateUserUseCase(IUserRepository userRepository,
        IRoleRepository roleRepository,
        IPasswordHasher passwordHasher,
        IAccessAssignmentService accessAssignmentService,
        IMenuRepository menuRepository)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _passwordHasher = passwordHasher;
        _accessAssignmentService = accessAssignmentService;
        _menuRepository = menuRepository;
    }


    public async Task<ApiResponse<UserDTO>> Execute(CreateUserRequest request)
    {
        User? user = await _userRepository.GetByEmailAsync(request.Email);

        if (user != null)
        {
            return new ApiResponse<UserDTO>(false, "User with the same email already exists.", null);
        }

        Role? role = await _roleRepository.GetByIdAsync(request.RoleId);
        if (role == null)
        {
            return new ApiResponse<UserDTO>(false, "Role not found.", null);
        }

        string hashedPassword = _passwordHasher.Hash(_passwordHasher.GetRandomGeneratedPassword());

        User newUser = new User
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            Password = hashedPassword,
            RoleId = request.RoleId,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        User createdUser = await _userRepository.AddAsync(newUser);

        UserDTO userDTO = new UserDTO(
            createdUser.Id.ToString(),
            createdUser.FirstName,
            createdUser.LastName,
            createdUser.Email,
            role.Name,
            createdUser.RoleId
        );

        IEnumerable<Menu> defaultMenus = await _menuRepository.GetMenusByUserHierarchyAsync(role.Hierarchy);
        List<MenuDTO> defaultMenuDTOs = defaultMenus.Select(m => new MenuDTO
        (
            IdMenu: m.Id,
            Name: m.Name,
            Url: m.Url,
            IsMain: m.IsMain,
            ParentId: m.ParentMenuId,
            MinHierarchy: m.MinimumHierarchy,
            Children: Array.Empty<MenuDTO>()
        )).ToList();


        // Asignar menús por defecto al usuario según su jerarquía
        MenuToUserDTO defMenus = await _accessAssignmentService.AssignDefaultMenusToUserAsync(
            createdUser.Id.ToString(),
            defaultMenuDTOs
        );
        // Por ahora no manejamos en caso de que los menus no se agregaron correctamente.
        return new ApiResponse<UserDTO>(true, "User created successfully.", userDTO);

    }


}
