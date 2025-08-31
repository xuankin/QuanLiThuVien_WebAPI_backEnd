# 📚 Library Management API

Dự án **Library Management API** được phát triển bằng **ASP.NET Core Web API (.NET 8.0)**.  
Hệ thống cung cấp API để quản lý thư viện, hỗ trợ xác thực người dùng với **ASP.NET Core Identity** và kết nối **SQL Server** thông qua **Entity Framework Core (Code First)**.

---

## 🚀 Công nghệ sử dụng
- **ASP.NET Core 8.0** (Web API)
- **Entity Framework Core** (Code First, Migrations)
- **SQL Server** (lưu trữ dữ liệu)
- **ASP.NET Core Identity** (xác thực & phân quyền người dùng)
- **Swagger / OpenAPI** (tài liệu API)
- **CORS** (cho phép truy cập từ nhiều nguồn)
- **Dependency Injection** (DI)

---

## ⚙️ Cấu hình
### 1. Chuẩn bị môi trường
- Cài đặt **.NET 8 SDK**
- Cài đặt **SQL Server** (hoặc Azure SQL)
- IDE khuyến nghị: **Visual Studio 2022** hoặc **Visual Studio Code**

### 2. Connection String
Trong file **appsettings.json**, cấu hình `DefaultConnection`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER;Database=LibraryDb;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

### 3. Migration & Database Update
Chạy lệnh sau trong **Package Manager Console** hoặc **Terminal**:

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

---

## 🔑 Authentication & Authorization
- Sử dụng **ASP.NET Core Identity** để quản lý tài khoản, đăng ký/đăng nhập.
- Hỗ trợ **phân quyền** (Role-based Authorization: Admin/User).

---

## 📖 API Endpoints chính
Một số API chính của hệ thống:

### Authentication
- `POST /api/auth/register` → Đăng ký tài khoản
- `POST /api/auth/login` → Đăng nhập

### Sách (Books)
- `GET /api/books` → Lấy danh sách sách
- `GET /api/books/{id}` → Xem chi tiết sách
- `POST /api/books` → Thêm sách mới (Admin)
- `PUT /api/books/{id}` → Cập nhật thông tin sách (Admin)
- `DELETE /api/books/{id}` → Xóa sách (Admin)

### Người dùng (Users)
- `GET /api/users` → Lấy danh sách người dùng (Admin)
- `GET /api/users/{id}` → Thông tin người dùng
- `DELETE /api/users/{id}` → Xóa người dùng (Admin)

### Quản lý mượn sách (Borrow/Return)
- `POST /api/borrow` → Mượn sách
- `POST /api/return` → Trả sách
- `GET /api/borrow/history` → Xem lịch sử mượn sách

📂 Cấu trúc thư mục chính
LibraryManagementApi/
│-- Controllers/         # Chứa các API Controller
│-- Data/                # DbContext và cấu hình database
│-- Models/              # Các entity class (Book, User, BorrowRecord,...)
│-- Migrations/          # EF Core Migration files
│-- Program.cs           # File khởi tạo ứng dụng
│-- appsettings.json 
