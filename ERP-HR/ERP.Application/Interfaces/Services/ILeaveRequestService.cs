using ERP.Application.DTOs;
using ERP.Domain.Enums;

namespace ERP.Application.Interfaces;

/// <summary>สัญญาของ business logic ฝั่งใบลา (workflow อนุมัติ + คำนวณวันลา + จัดการโควต้า)</summary>
public interface ILeaveRequestService
{
    Task<List<LeaveRequestResponse>> GetByEmployeeAsync(Guid employeeId, LeaveRequestStatus? status, int? year, CancellationToken ct = default);
    Task<LeaveRequestResponse?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<LeaveRequestResponse> CreateAsync(CreateLeaveRequestRequest request, CancellationToken ct = default);
    Task<LeaveRequestResponse> ApproveAsync(Guid id, ApproveLeaveRequestRequest request, CancellationToken ct = default);
    Task<LeaveRequestResponse> RejectAsync(Guid id, RejectLeaveRequestRequest request, CancellationToken ct = default);
    Task CancelAsync(Guid id, CancellationToken ct = default);
}
