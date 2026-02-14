namespace menu_users.Application.DTOs.ApiResponse;

public record ApiResponse<T>
(
    bool Success,
    string? Error,
    T? Data
);