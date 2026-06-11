namespace ERP.Application.DTOs;

/// <summary>
/// ข้อมูลขาเข้าสำหรับเปลี่ยนรหัสผ่าน (แยกจาก UpdateUserRequest เพราะเป็น action ที่ต้องยืนยันตัวตน)
/// service จะตรวจ CurrentPassword กับ hash เดิม แล้ว hash NewPassword ก่อนเรียก User.ChangePassword
/// </summary>
public record ChangePasswordRequest
{
    public required string CurrentPassword { get; init; }
    public required string NewPassword { get; init; }
}