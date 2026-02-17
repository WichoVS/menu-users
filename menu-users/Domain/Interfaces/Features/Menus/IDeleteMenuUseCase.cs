using System;
using menu_users.Application.DTOs.ApiResponse;

namespace menu_users.Domain.Interfaces.Features.Menus;

public interface IDeleteMenuUseCase
{
    Task<ApiResponse<bool>> Execute(int id);
}
