namespace ERP.Application.Common.Exceptions;

/// <summary>โยนเมื่อหา resource ที่ระบุไม่เจอ → จะถูก map เป็น HTTP 404 ที่ชั้น API</summary>
public class NotFoundException(string message) : Exception(message);
