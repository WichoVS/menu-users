namespace menu_users.Application.DTOs.ApiResponse;

public record class ApiResponse<T>
(
    bool Success,
    string? Error,
    T? Data
);