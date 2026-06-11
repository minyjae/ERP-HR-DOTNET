namespace ERP.Application.Interfaces;

/// <summary>
/// abstraction ของการ hash/ตรวจรหัสผ่าน — ประกาศใน Application แต่ implement ใน Infrastructure
/// domain/service ไม่ผูกกับอัลกอริทึม hash โดยตรง (เปลี่ยน implementation ได้โดยไม่กระทบ business logic)
/// </summary>
public interface IPasswordHasher
{
    /// <summary>hash รหัสผ่าน plain text → string ที่เก็บลง DB ได้ (ฝัง salt + iterations ไว้ในตัว)</summary>
    string Hash(string password);

    /// <summary>ตรวจว่า plain text ตรงกับ hash ที่เก็บไว้ไหม (เปรียบเทียบแบบ fixed-time กัน timing attack)</summary>
    bool Verify(string password, string hash);
}
