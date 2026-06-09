namespace ERP.Application.Common.Exceptions;

/// <summary>โยนเมื่อข้อมูลชนกับที่มีอยู่ เช่น EmployeeCode/Email ซ้ำ → map เป็น HTTP 409 ที่ชั้น API</summary>
public class ConflictException(string message) : Exception(message);
