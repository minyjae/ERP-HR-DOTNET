# ERP-HR-DOTNET

รัน PostgreSQL ก่อน (วิธีง่ายสุดถ้ามี Docker):
docker run --name erp-pg -e POSTGRES_PASSWORD=postgres -e POSTGRES_DB=erp_hr -p 5432:5432 -d postgres
dotnet ef database update   # สร้าง table จาก migration
dotnet run                  # เปิด API
แล้วยิงทดสอบจากไฟล์ ERP-HR.http ได้เลย (มีตัวอย่างครบทุก endpoint) — ถ้า connection string/รหัสผ่านต่างไป แก้ที่ appsettings.json