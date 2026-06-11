using ERP.Application.DTOs;
using ERP.Domain.Entities;

namespace ERP.Application.Mappings;

/// <summary>แปลง LeaveRequest entity → DTO (สไตล์เดียวกับ Branch/Employee)</summary>
public static class LeaveRequestMappingExtensions
{
    public static LeaveRequestResponse ToResponse(this LeaveRequest r) => new()
    {
        Id = r.Id,
        EmployeeId = r.EmployeeId,
        LeaveTypeId = r.LeaveTypeId,
        StartDate = r.StartDate,
        EndDate = r.EndDate,
        TotalDays = r.TotalDays,
        Reason = r.Reason,
        AttachmentUrl = r.AttachmentUrl,
        Status = r.Status,
        RequestedAt = r.RequestedAt,
        ApprovedById = r.ApprovedById,
        ApprovedAt = r.ApprovedAt,
        RejectReason = r.RejectReason,
        CancelledAt = r.CancelledAt,
        UpdatedAt = r.UpdatedAt
    };
}
