using ERP.Application.DTOs;
using ERP.Domain.Enums;

namespace ERP.Application.Interfaces;

/// <summary>
/// สัญญาของ business logic ฝั่ง User
/// controller คุยกับ interface นี้เท่านั้น ไม่ยุ่งกับ repository หรือ password hasher โดยตรง
/// </summary>
public interface IUserService
{
    Task<List<UserResponse>> GetAllAsync(CancellationToken ct = default);
    Task<List<UserResponse>> GetByRoleAsync(UserRole role, CancellationToken ct = default);
    Task<UserResponse?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<UserResponse> CreateAsync(CreateUserRequest request, CancellationToken ct = default);
    Task<UserResponse> UpdateAsync(Guid id, UpdateUserRequest request, CancellationToken ct = default);
    Task ChangePasswordAsync(Guid id, ChangePasswordRequest request, CancellationToken ct = default);
    Task ActivateAsync(Guid id, CancellationToken ct = default);
    Task DeactivateAsync(Guid id, CancellationToken ct = default);
    Task DeleteAsync(Guid id, CancellationToken ct = default);
}
